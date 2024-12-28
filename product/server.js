// server.js
const express = require('express');
const mongoose = require('mongoose');
const dotenv = require('dotenv');
const productRoutes = require('./src/routes/productRoutes');
const { DaprServer } = require('@dapr/dapr');

// Charger les variables d'environnement
dotenv.config();

const app = express();
const port = process.env.PORT || 3003;

// Configurer Dapr
const daprServer = new DaprServer("127.0.0.1", 3500, "127.0.0.1", port);

// Middleware pour traiter les données JSON
app.use(express.json());

// Connexion à la base de données MongoDB
mongoose.connect(process.env.MONGO_URI, {
  useNewUrlParser: true,
  useUnifiedTopology: true
}).then(() => {
  console.log('MongoDB connected');
}).catch((err) => {
  console.error('MongoDB connection error:', err);
  process.exit(1); // Arrêter l'application si MongoDB échoue
});

// Routes
app.use('/products', productRoutes);

// Démarrer le serveur Dapr
daprServer.start().then(() => {
  console.log('Dapr sidecar connected');
  // Démarrer le serveur Express une fois que Dapr est prêt
  app.listen(port, () => {
    console.log(`Server running on port ${port}`);
  });
}).catch((err) => {
  console.error('Error starting Dapr server:', err);
  process.exit(1);
});

// Route par défaut
app.get('/', (req, res) => {
  res.send('Microservice Product fonctionne avec Dapr');
});
