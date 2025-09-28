# 🛒 ESourcing Microservices Project

This repository contains a microservices-based e-commerce sourcing system built with **ASP.NET Core 8**, following **Clean Architecture** and **Domain-Driven Design (DDD)** principles.  
The project was developed as part of a Udemy course, with additional improvements, Docker integration, and refactoring.

---

## 📌 Project Overview
ESourcing simulates an online auction system where buyers and sellers interact.  
It demonstrates:
- **Microservices architecture** with independent services  
- **Event-driven communication** using **RabbitMQ**  
- **API Gateway** for unified API access via **Ocelot**  
- **Authentication & Authorization** with **IdentityServer** and **Google OAuth**  
- **Containerization** with **Docker & Docker Compose**  
- **Scalability** using **CQRS** and **Mediator** patterns  

---

## 🏗️ Architectures & Patterns
- **Architectures**: Hybrid, Microservices, Clean  
- **Design Patterns**:  
  - CQRS  
  - DDD (Domain Driven Design)  
  - Mediator  
  - Repository Pattern  
  - Dependency Injection  
  - Event Driven  
  - API Gateway  

---

## 🗄️ Databases
- **MSSQL Server**  
- **MongoDB**  
- **RabbitMQ** (for event/message broker)

---

## 🔧 Technologies
- **ORM**: Entity Framework Core  
- **Security**: OAuth, IdentityServer  
- **Tools**:  
  - MongoDB Compass  
  - Docker / Docker Compose  
  - Portainer  
  - Postman  
  - Swagger  
  - JetBrains Rider  

---

## 🎨 Frontend
- ASP.NET Core MVC  
- Razor Pages  
- Bootstrap / CSS / HTML / JavaScript / jQuery  
- FontAwesome, Google Fonts  
- SignalR for real-time communication  

---

## 📦 NuGet Packages
- AutoMapper  
- MediatR (+ Extensions)  
- FluentValidation  
- Microsoft.AspNetCore.Authentication.Google  
- Microsoft.AspNetCore.Identity.EntityFrameworkCore  
- Microsoft.EntityFrameworkCore.* (Core, SqlServer, Sqlite, Design, InMemory)  
- MongoDB.Driver  
- Newtonsoft.Json  
- Ocelot  
- Polly  
- RabbitMQ.Client  
- Swashbuckle.AspNetCore (Swagger)  

---

## 🐳 Dockerized Services
- **esourcingproducts** – Product service  
- **esourcingsourcing** – Auction/Bid service  
- **esourcingorder** – Order service  
- **esourcingapigateway** – Ocelot API Gateway  
- **esourcingui** – ASP.NET MVC UI  
- **sourcingdb** – MongoDB container  
- **sourcingsqldb** – SQL Server container  
- **rabbitmq** – Message broker  
- **portainer** – Container management  

👉 Each service runs independently in its own container and communicates through REST APIs or RabbitMQ.

---

## 📂 Project Structure
ESourcing
┣ ESourcing.APIGateway
┣ ESourcing.Core
┣ ESourcing.Infrastructure
┣ ESourcing.Order
┣ ESourcing.Order.Application
┣ ESourcing.Order.Domain
┣ ESourcing.Order.Infrastructure
┣ ESourcing.Products
┣ ESourcing.Sourcing
┣ ESourcing.UI
┣ EventBusRabbitMQ
┣ compose.yaml

---

## 🚀 How to Run
1. Clone this repository:
   git clone https://github.com/RumpleSteelSkin/ESourcing.git
   cd ESourcing
   
2. Run with Docker Compose:
   docker-compose up -d --build

API Gateway → http://localhost:5000
Product API → http://localhost:5001
Sourcing API → http://localhost:5002
Order API → http://localhost:5003
UI → http://localhost:8080

---