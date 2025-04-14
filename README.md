# 🛍️ Product Management API

A modern RESTful API built with **ASP.NET Core (.NET 8)** for managing products.  
Designed with clean architecture principles, custom exception handling, and full unit test coverage using **xUnit + Moq**.

---

## 🚀 Tech Stack

- ASP.NET Core Web API (.NET 8)
- Entity Framework Core (Code-First + In-Memory for Testing)
- SQL Server (LocalDB)
- xUnit & Moq (Unit Testing)
- Swagger (API Documentation)

---

## 📁 Features

- Create, Read, Update, Delete products
- Add and Decrement product stock
- Custom 6-digit unique `ProductId` generation
- Full audit fields: `CreatedBy`, `UpdatedBy`, `CreatedAt`, `UpdatedAt`
- Global exception handling with structured error responses
- Correlation ID support for tracing errors
- DTO-based request validation and Swagger cleanup

---

## 📦 Project Structure

```
ProductSolution/
├── ProductApi/           # Main API project
│   ├── Controllers/
│   ├── Models/
│   ├── DTOs/
│   ├── Interfaces/
│   ├── Services/
│   ├── Utilities/
│   ├── Middleware/
│   ├── Exceptions/
│   ├── Data/
│   └── Program.cs
└── ProductApi.Tests/     # xUnit + Moq test project
    ├── Services/
    └── Controllers/
```

---

## 🛠️ Getting Started

### ✅ Prerequisites
- Visual Studio 2022 or later
- .NET 8 SDK
- SQL Server (LocalDB is fine)

### 🧪 Setup & Run

1. Clone the repository
2. Open the solution in Visual Studio
3. Set `ProductApi` as the **Startup Project**
4. Open **Package Manager Console**:
   ```powershell
   Add-Migration InitialCreate
   Update-Database
   ```
5. Run the application
6. Open Swagger UI at:
   ```
   https://localhost:<port>/swagger
   ```

---

## 📬 Available Endpoints

| Method | Endpoint                                   | Description                     |
|--------|--------------------------------------------|---------------------------------|
| GET    | /api/products                              | Get all products                |
| GET    | /api/products/{id}                         | Get product by ID               |
| POST   | /api/products                              | Create a new product            |
| PUT    | /api/products/{id}                         | Update an existing product      |
| DELETE | /api/products/{id}                         | Delete a product                |
| PUT    | /api/products/decrement-stock/{id}/{qty}   | Decrease stock quantity         |
| PUT    | /api/products/add-to-stock/{id}/{qty}      | Increase stock quantity         |

> 🧼 Swagger input bodies are clean, thanks to DTO usage.

---

## 🧪 Testing

- Open **Test Explorer** in Visual Studio
- Run All Tests ✅
- Tests include:
  - ProductService (unit tested)
  - ProductController (unit tested using Moq)

---

## ❗ Error Handling

All exceptions are handled globally via middleware:
- Custom exceptions: `NotFoundException`, `BadRequestException`
- Clean JSON response format:
```json
{
  "statusCode": 404,
  "message": "Product with ID 999999 not found.",
  "correlationId": "abc123"
}
```

---

## 📎 Notes

- Product ID is auto-generated and unique (6-digit format)
- Fields like `ProductId`, `CreatedAt`, etc., are **not exposed in request bodies**
- In-memory DB is used for testing to ensure full isolation

---

## 👏 Author

Built and documented by someone who believes in clean architecture, testable services, and APIs that speak for themselves. ✨


Built with ❤️ by a .NET Developer who knows clean code, clean APIs, and clean tests.

---
