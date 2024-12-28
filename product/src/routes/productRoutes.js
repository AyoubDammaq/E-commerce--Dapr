// src/routes/productRoutes.js
const express = require('express');
const router = express.Router();
const { createProduct, getAllProducts, deleteProduct, updateProductDetails } = require('../controllers/productController');

// Route pour créer un produit
router.post('/', createProduct);

// Route pour lister tous les produits
router.get('/', getAllProducts);

router.delete('/:id', deleteProduct);

// Route pour mettre à jour les détails d'un produit
router.put('/:id', updateProductDetails);

module.exports = router;
