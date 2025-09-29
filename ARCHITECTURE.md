## 🏗️ Solution Structure

```plaintext
📦 TodoSphere (Solution)
│
├── 📂 TodoSphere.ApiGateway
│   ├── TodoSphere.ApiGateway.csproj   // YARP as API Gateway
│   └── Dockerfile
│
├── 📂 Services
│   ├── 📂 Auth
│   │   ├── TodoSphere.Auth.API
│   │   │   └── Dockerfile
│   │   ├── TodoSphere.Auth.Application
│   │   ├── TodoSphere.Auth.Infrastructure
│   │   └── TodoSphere.Auth.Domain
│   │
│   ├── 📂 Users
│   │   ├── TodoSphere.Users.API
│   │   │   └── Dockerfile
│   │   ├── TodoSphere.Users.Application
│   │   ├── TodoSphere.Users.Infrastructure
│   │   └── TodoSphere.Users.Domain
│   │
│   ├── 📂 Tasks
│   │   ├── TodoSphere.Tasks.API
│   │   │   └── Dockerfile
│   │   ├── TodoSphere.Tasks.Application
│   │   ├── TodoSphere.Tasks.Infrastructure
│   │   └── TodoSphere.Tasks.Domain
│   │
│   └── 📂 Notifications
│       ├── TodoSphere.Notifications.API
│       │   └── Dockerfile
│       ├── TodoSphere.Notifications.Application
│       ├── TodoSphere.Notifications.Infrastructure
│       └── TodoSphere.Notifications.Domain
│
└── 📄 docker-compose.yml
```
