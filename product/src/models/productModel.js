// src/models/productModel.js
const mongoose = require('mongoose');

// Définir le schéma du produit
const productSchema = new mongoose.Schema({
  name: { type: String, required: true },
  description: { type: String },
  price: { type: Number, required: true },
  quantity: { type: Number, required: true },
});


// Créer et exporter le modèle du produit
const Product = mongoose.model('Product', productSchema);
module.exports = Product;
