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
<img width="1913" height="893" alt="Signin" src="https://github.com/user-attachments/assets/99c44c71-3493-4bf0-8635-af435ca20c54" />
<img width="1603" height="392" alt="Login" src="https://github.com/user-attachments/assets/228dd57d-511a-46bd-9f20-f705d5850907" />
<img width="1902" height="493" alt="Auctions" src="https://github.com/user-attachments/assets/97d3d5fe-8b44-4866-a600-c89ea605486a" />
<img width="1907" height="687" alt="Auction-1" src="https://github.com/user-attachments/assets/b193af88-4359-48bb-9133-6920d431310e" />
<img width="1647" height="408" alt="Auction-2" src="https://github.com/user-attachments/assets/d8d06875-83a6-4e20-aa75-9992833e3888" />
<img width="1642" height="270" alt="Auction-3" src="https://github.com/user-attachments/assets/fb543f08-ac19-407a-bcaa-db973f2026d6" />
<img width="1907" height="1021" alt="Bids" src="https://github.com/user-attachments/assets/65dee501-aac9-4c5d-ab12-1fc254be86cf" />
<img width="1647" height="405" alt="Auction Closed" src="https://github.com/user-attachments/assets/7579b47c-1aa4-4c03-8fc6-f5d263606ad6" />
<img width="1906" height="958" alt="Portainer" src="https://github.com/user-attachments/assets/a4b53b31-da47-40ca-a965-81d2567a9275" />
<img width="1892" height="720" alt="Docker" src="https://github.com/user-attachments/assets/a6815206-510b-43b0-bd23-bcd66634a581" />
<img width="1006" height="911" alt="Folder Structure" src="https://github.com/user-attachments/assets/3f397669-7e82-426a-a4ed-6e216f48539f" />

