// src/config/database.js
const mongoose = require('mongoose');
const dotenv = require('dotenv');

dotenv.config();

const connectDB = async () => {
  try {
    await mongoose.connect(process.env.MONGO_URI); // No need for useNewUrlParser and useUnifiedTopology
    console.log('MongoDB connected');
  } catch (error) {
    console.error('Database connection error', error);
    process.exit(1);
  }
};

module.exports = connectDB;
