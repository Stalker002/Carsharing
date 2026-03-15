# 🚗 Carsharing Platform API

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)

A robust, scalable, and high-performance REST API for a Carsharing (Car Rental) service. Built with **.NET 9**, following **Clean Architecture** principles, and fully containerized using **Docker**.

## 📌 About The Project

This backend system handles the entire lifecycle of a carsharing business. It provides endpoints for user registration, document verification, fleet management, real-time booking, automated billing, and penalty tracking.

The architecture is designed to be maintainable, testable, and highly optimized for database queries.

### 🔥 Key Features
* **Role-Based Authorization:** Secure JWT-based authentication for `Admin` and `Client` roles.
* **Automated Billing System:** Calculates trip costs based on duration, distance, and applied promo codes.
* **S3 Cloud Storage:** Integration with **MinIO** for secure storage of user documents and car images.
* **Optimized Performance:** Eliminated N+1 problems and in-memory joins using EF Core DTO projections (`.Select()`).
* **Transactional Integrity:** Safe data manipulation (e.g., booking a car and updating its status) using the **Unit of Work** and **Repository** patterns.
* **Robust Error Handling:** Global Exception Middleware for standardized API responses (RFC 7807 Problem Details).

## 🏗 Architecture

The project strictly follows **Clean Architecture** to separate business rules from infrastructure:
1. `Carsharing.Core` (Domain): Entities, Enums, Custom Exceptions, and Interfaces.
2. `Carsharing.Application` (Use Cases): Business logic, Services, DTOs, and JWT Providers.
3. `Carsharing.DataAccess` (Infrastructure): EF Core `DbContext`, Database Migrations, and Repository implementations.
4. `Carsharing` (Presentation/API): Controllers, Middleware, and Dependency Injection setup.

## 🛠 Tech Stack

* **Framework:** .NET 9 / ASP.NET Core Web API
* **Database:** PostgreSQL & Entity Framework Core (Code-First)
* **Storage:** MinIO (Amazon S3 compatible)
* **Security:** JWT (JSON Web Tokens), BCrypt Password Hashing, ASP.NET Core Data Protection
* **Containerization:** Docker & Docker Compose
* **Testing:** xUnit, Moq, In-Memory Database (for Integration Tests)

## 🚀 Getting Started

To run this project locally, you need [Docker](https://www.docker.com/products/docker-desktop) installed on your machine.

### 1. Clone the repository
```bash
git clone https://github.com/Stalker002/Carsharing.git
cd Carsharing
```

### 2. Run with Docker Compose
The `docker-compose.yml` file includes the API, PostgreSQL database, and MinIO storage.

```bash
docker-compose up -d --build
```

### 3. Explore the API
Once the containers are running, the API will automatically apply database migrations. You can explore the endpoints using Swagger UI:

* **Swagger UI:** `http://localhost:5078/swagger`
* **MinIO Console:** `http://localhost:9001` *(Username: `minioadmin` / Password: `minioadmin`)*

## 🧪 Testing

The project includes both **Unit Tests** (for Domain Models and Services) and **Integration Tests** (for Controllers and Database interactions).

To run the tests:
```bash
dotnet test
```

## 🗄️ Database Schema Summary
The relational database is highly normalized and includes the following main entities:
* `Users` & `Clients` (Profile & Documents)
* `Cars`, `Categories`, `Tariffs`, `Specifications`
* `Bookings` & `Trips` (With automated status tracking)
* `Bills`, `Payments`, `Promocodes`, `Fines`
* `Favorites` & `Reviews`

## 👨‍💻 Author

**<Твое Имя/Фамилия>**
* LinkedIn: [Link LinkedIn](https://www.linkedin.com/in/michail-kolupaev/)
* Email: michael.kolupaev@gmail.com
