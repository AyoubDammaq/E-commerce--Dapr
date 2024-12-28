// src/models/productModel.js
const mongoose = require('mongoose');

// Définir le schéma du produit
const productSchema = new mongoose.Schema({
  name: {
    type: String,
    required: true
  },
  price: {
    type: Number,
    required: true
  },
  description: String,
  category: String
});

// Créer et exporter le modèle du produit
const Product = mongoose.model('Product', productSchema);
module.exports = Product;
