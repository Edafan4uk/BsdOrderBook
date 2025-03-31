# BsdOrderBook API

## Overview
BsdOrderBook is a .NET 8 Web API application designed for order book management. This project includes Docker support for easy deployment and testing.

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/) (optional)

## 🚀 Running the Application with Docker

### **1. Build the Docker Image**
```sh
docker build -t bsdorderbook .
```
2. Run the Docker Container
```
docker run -it --rm -p 8080:8080 --name bsdorderbook bsdorderbook
```
## 🐳 Running with Docker Compose
To simplify running the application, you can use Docker Compose:

1. Start the Application
```
docker-compose up --build
```
2. Stop the Application
```
docker-compose down
```
## Access the API
Swagger UI: http://localhost:8080/swagger
