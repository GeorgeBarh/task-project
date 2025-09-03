# TaskMeUp

TaskMeUp is a **full-stack task management application** with a modern Angular frontend and a secure ASP.NET Core 9 backend.  
It allows users to manage groups and tasks collaboratively with JWT-based authentication.

---

## Features

- 🔑 **Authentication** – Register, login, update profile, delete account  
- 👥 **Groups** – Create, update, delete groups, manage group membership  
- ✅ **Tasks** – Create, update, delete tasks within groups  
- 🔒 **Secure API** – JWT authentication with BCrypt password hashing  
- 📖 **API Docs** – Swagger UI available for easy testing  
- 🧪 **Testing** – xUnit & Moq (API), Karma/Jasmine (Client)  

---

## Tech Stack

### Backend (TaskMeUp.Api)
- ASP.NET Core 9
- Entity Framework Core 9 (SQL Server)
- JWT Authentication
- Swagger / OpenAPI
- xUnit + Moq for testing

### Frontend (TaskMeUp.Client)
- Angular 19
- TypeScript 5.7
- Angular Material / CDK
- Karma + Jasmine for testing

---

## Prerequisites

Make sure you have the following installed:

- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express, LocalDB, or container)
- [Node.js](https://nodejs.org/) (v20 or newer recommended)
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Angular CLI (install globally):

```bash
npm install -g @angular/cli
