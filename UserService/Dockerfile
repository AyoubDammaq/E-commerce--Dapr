# Step 1: Use an official Java runtime as a base image
FROM openjdk:22-jdk-slim AS base

# Step 2: Set environment variables
ENV JAVA_OPTS="-Xms512m -Xmx1024m"

# Step 3: Set the working directory in the container
WORKDIR /app

# Step 4: Copy the .jar file into the container
COPY ./target/springboot-security-jwt-*.jar app.jar

# Step 5: Expose the port your app will run on (default Spring Boot port is 8080)
EXPOSE 5002

# Step 6: Command to run the application
ENTRYPOINT ["java", "-jar", "/app/app.jar"]
