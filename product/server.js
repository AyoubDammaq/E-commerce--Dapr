// src/server.js
const express = require('express');
const mongoose = require('mongoose');
const dotenv = require('dotenv');
const productRoutes = require('./src/routes/productRoutes');
const axios = require('axios');
const connectDB = require('./src/config/database');
const Product = require('./src/models/productModel');

dotenv.config();

const app = express();
const port = process.env.PORT || 3003;
const daprPort = process.env.DAPR_PORT || 3502;
const stateUrl = `http://localhost:${daprPort}/v1.0/state/statestore`;

app.use(express.json());

// Connecter MongoDB
connectDB();

// Abonnement Dapr pour traiter les commandes
app.post('/product-pubsub/order-placed', async (req, res) => {
  const order = req.body;
  try {
    console.log(`Commande reçue : Produit ID ${order.ProductId}, Quantité ${order.Quantity}`);

    const product = await Product.findById(order.ProductId);
    if (product) {
      if (order.Quantity <= product.quantity) {
        product.quantity -= order.Quantity;
        await product.save();
        console.log(`Produit ${product.name} mis à jour, nouvelle quantité : ${product.quantity}`);
        res.status(200).send('Product updated');
      } else {
        console.log('Quantité commandée supérieure à la quantité en stock');
        res.status(400).send('Insufficient stock');
      }
    } else {
      console.log('Produit non trouvé');
      res.status(404).send('Product not found');
    }
  } catch (error) {
    console.error('Erreur lors du traitement de la commande :', error);
    res.status(500).send('Error processing order');
  }
});

// Point de découverte Dapr
app.get('/dapr/subscribe', (req, res) => {
  res.json([
    {
      pubsubname: 'product-pubsub',
      topic: 'order-placed',
      route: '/product-pubsub/order-placed',
    },
  ]);
});

// Routes produits
app.use('/products', productRoutes);

// Démarrer le serveur Express
app.listen(port, () => {
  console.log(`Server running on port ${port}`);
});

// Fermeture propre
process.on('SIGINT', async () => {
  console.log('Shutting down...');
  await mongoose.disconnect();
  process.exit(0);
});