// src/routes/productRoutes.js
const express = require('express');
const router = express.Router();
const { createProduct, getAllProducts, deleteProduct } = require('../controllers/productController');

// Route pour créer un produit
router.post('/', createProduct);

// Route pour lister tous les produits
router.get('/', getAllProducts);

router.delete('/:id', deleteProduct);

module.exports = router;
