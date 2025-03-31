# BsdOrderBook API

## Overview

BsdOrderBook is a .NET 8 application implementing a meta-exchange for optimizing Bitcoin (BTC) trades across multiple crypto exchanges. It finds the best price for buying or selling BTC while considering exchange-specific balance constraints.

The application consists of:
- A **console application** computes optimal trade execution.
- A **web API** (ASP.NET Core) that exposes this functionality via a REST endpoint.
- **Docker support** for easy deployment.

The system ensures that BTC trades are executed at the most favorable rates without transferring funds between exchanges.

## 📂 Order Books Data

The application reads order books from the `order_books_data` file. This file contains multiple JSON entries, each representing an order book from a different exchange. The data is used to determine the best possible execution strategy while respecting exchange-specific balance constraints.

### **File Format**
Each row in the file consists of a timestamp followed by a JSON object containing bid and ask orders. Example:
```
1548759600.25189 { "AcqTime": "2019-01-29T11:00:00.2518854Z", "Bids": [ { "Order": { "Id": null, "Time": "0001-01-01T00:00:00", "Type": "Buy", "Kind": "Limit", "Amount": 0.01, "Price": 2960.64 } } ], "Asks": [ { "Order": { "Id": null, "Time": "0001-01-01T00:00:00", "Type": "Sell", "Kind": "Limit", "Amount": 0.405, "Price": 2964.29 } } ] }
```

### **Fields**
- **AcqTime**: The timestamp when the order book was retrieved.
- **Bids**: List of buy orders (price the buyer is willing to pay).
- **Asks**: List of sell orders (price the seller is willing to accept).
- **Amount**: BTC quantity available at the given price.
- **Price**: The bid/ask price in EUR.

The system processes this data to determine the best possible order execution strategy.


## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/) (optional)

## 🔧 Development & Debugging
To run the application locally without Docker:
```
dotnet run --project src/BsdOrderBook.Host
```
## Access the API
Swagger UI: http://localhost:8080/swagger

## 🚀 Running the Application with Docker

**1. Build the Docker Image**
```sh
docker build -t bsdorderbook .
```
**2. Run the Docker Container**
```
docker run -it --rm -p 8080:8080 --name bsdorderbook bsdorderbook
```
## 🐳 Running with Docker Compose
To simplify running the application, you can use Docker Compose:

**1. Start the Application**
```
docker-compose up --build
```
**2. Stop the Application**
```
docker-compose down
```
## 🧪 Running Tests with .NET CLI
To run all tests using the .NET CLI:
```
dotnet test
```
