# Étape 1 : Utiliser une image de base Java
FROM openjdk:17-jdk-slim as builder

# Étape 2 : Créer un répertoire pour l'application
RUN mkdir /app
WORKDIR /app

# Étape 3 : Copier le fichier jar de l'application dans le conteneur
COPY target/user-service.jar /app/user-service.jar

# Étape 4 : Exposer le port sur lequel l'application Spring Boot écoute
EXPOSE 8080

# Étape 5 : Démarrer l'application Spring Boot
CMD ["java", "-jar", "user-service.jar"]
