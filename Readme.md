# OA System

A leave application management system built with .NET 9 and Vue 3.

## Prerequisites

Before you begin, ensure you have met the following requirements:

- Node.js 18+
- .NET 9.0
- SQL Server instance or Docker
- Docker (optional, for containerized SQL Server)

## Getting Started

### 1. Setting up MSSQL with Docker

Start SQL Server using Docker:

```sh
docker-compose -f docker-compose.dev.yml up -d
```

Verify the container is running:
```sh
docker ps
```

### 2. Setting up the Backend (.NET)

1. Navigate to the OaService directory:
```sh
cd OaService
```

2. Install dependencies:
```sh
dotnet restore
```

3. Run EF migrations:
```sh
dotnet ef database update
```

4. Start the backend service:
```sh
dotnet run
```

The backend will be available at `http://localhost:5237`.

### 3. Setting up the Frontend (Vue)

1. Navigate to the OaClient directory:
```sh
cd OaClient
```

2. Install dependencies:
```sh
npm install
```

3. Start the development server:
```sh
npm run dev
```

The frontend will be available at `http://localhost:3000`.

## Default Login Credentials

The system is seeded with two default users:

### Manager Account
- Email: manager@example.com
- Password: Manager123!
- Role: Manager

### Staff Account
- Email: staff@example.com
- Password: Staff123!
- Role: Staff

## Features

- JWT Authentication
- Role-based authorization
- Leave application management
- Manager approval workflow
- Real-time status updates

## Development Workflow

1. Start SQL Server (if using Docker):
```sh
docker-compose -f docker-compose.dev.yml up -d
```

2. Start the backend (.NET):
```sh
cd OaService
dotnet run
```

3. Start the frontend (Vue):
```sh
cd OaClient
npm run dev
```