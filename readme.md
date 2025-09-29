# 📦 TodoSphere

TodoSphere is a **microservices-based solution** designed to manage **tasks, users, authentication, and notifications**, all orchestrated through an **API Gateway powered by YARP**.
The architecture follows **Clean Architecture** principles, ensuring scalability, modularity, and maintainability.

---

## 🚀 Features

- ✅ **API Gateway** with YARP (Yet Another Reverse Proxy)
- 🔑 **Authentication Service** (JWT, user login/registration)
- 👥 **User Service** (profile and user management)
- 📋 **Tasks Service** (task creation, updates, assignments)
- 🔔 **Notifications Service** (system and user notifications)
- 🐳 **Dockerized** (each service with its own Dockerfile)
- ⚙️ **docker-compose orchestration** for local development

---

# ⚙️ Requirements

- .NET 8 SDK
- Docker & Docker Compose

---

# ▶️ Getting Started

### 1️⃣ Clone the repository

```bash
git clone https://github.com/Wil-JsDev/TodoSphere.git
cd TodoSphere
```

### 2️⃣ Run the solution with Docker Compose

```yml
docker-compose up --build
```

### 3️⃣ Access the services

API Gateway → https://localhost:5000

Auth Service → https://localhost:5001

Users Service → https://localhost:5002

Tasks Service → https://localhost:5003

Notifications Service → https://localhost:5004

### 🛠️ Tech Stack

- .NET 8 (ASP.NET Core Web API)

- YARP (API Gateway)

- Entity Framework Core

- Docker & Docker Compose

- PostgreSQL | MongoDB

- Redis
