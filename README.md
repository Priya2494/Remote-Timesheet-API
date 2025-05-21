# 🕒 Remote Timesheet API

A clean, professional .NET 8 Web API for logging remote work hours — built with Entity Framework Core, Swagger, and ready for Azure deployment. Designed for freelancers, remote teams, or internal company use.

---

## 🚀 Features

- ✅ Log, update, and delete timesheet entries
- ✅ RESTful API using ASP.NET Core 8
- ✅ Entity Framework Core + SQL Server (LocalDB)
- ✅ Swagger UI for easy testing
- ✅ Pagination support with deferred vs immediate execution
- ✅ CI/CD ready (via GitHub Actions)
- ✅ Deployable to Azure App Service (Free Tier)

---

## 🛠 Tech Stack

| Layer        | Tech                                 |
|--------------|--------------------------------------|
| Backend      | ASP.NET Core 8                       |
| ORM          | Entity Framework Core                |
| Database     | SQL Server LocalDB                   |
| Docs         | Swagger / OpenAPI                    |
| DevOps       | Git + GitHub + GitHub Actions        |
| Deployment   | Azure App Service (optional)         |

---

## 📦 Getting Started

### ✅ Clone the Repo

```bash
git clone https://github.com/YOUR_USERNAME/Remote-Timesheet-API.git
cd Remote-Timesheet-API/TimesheetAPI

dotnet ef database update

dotnet run

https://localhost:xxxx/swagger

(Replace xxxx with the port shown in your terminal.)

API Endpoints

| Method | Endpoint                         | Description                      |
| ------ | -------------------------------- | -------------------------------- |
| GET    | `/api/timesheet`                 | Get all timesheet entries        |
| POST   | `/api/timesheet`                 | Create a new timesheet entry     |
| PUT    | `/api/timesheet/{id}`            | Update an entry                  |
| DELETE | `/api/timesheet/{id}`            | Delete an entry                  |
| GET    | `/api/timesheet/paged-deferred`  | Paged fetch (deferred execution) |
| GET    | `/api/timesheet/paged-immediate` | Paged fetch (in-memory)          |

## 🌐 Deployment (Azure)

> This app can be deployed to Azure App Service (Free Tier) via GitHub Actions.  
> Live demo link: *Coming soon...*

## 🔄 CI/CD (Optional Setup)

> GitHub Actions workflow can be added to validate builds and prepare for deployment.

---

## 👤 Author

**Priyanka S**  
.NET Developer – 9.8 years experience  
💼 [LinkedIn](#) • 💻 [GitHub](#)

---

## 📄 License

MIT License — free to use, modify, and share.

## 📸 Swagger Screenshot

![Swagger Screenshot](relative-path-or-upload-url)
