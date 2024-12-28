# Utiliser une image Node.js officielle
FROM node:18

# Définir le répertoire de travail
RUN mkdir -p /app
WORKDIR /app


# Copier les fichiers package.json et package-lock.json
COPY package*.json ./

# Installer les dépendances
RUN npm install

# Copier le reste des fichiers du microservice
COPY . .

# Exposer le port sur lequel l'application fonctionne
EXPOSE 3003

# Commande pour démarrer le microservice
CMD ["npm", "start"]
