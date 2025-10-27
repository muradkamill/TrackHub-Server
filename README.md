## 🚀 TrackHub API: Modern Shipment & Order Tracking System

![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-Programming-orange?logo=csharp)
![Architecture](https://img.shields.io/badge/Architecture-Clean-blue)
![License](https://img.shields.io/badge/License-MIT-green)

> **TrackHub** is a robust backend service designed for a high-performance **shipment and order tracking** system. It is built using **ASP.NET Core Web API** and adheres to **Clean Architecture** principles for maximum scalability and maintainability.

---

## ✨ Key Features

This project is engineered with a focus on security, scalability, and essential business functionality:

* 🔐 **Secure Authentication:** JWT (JSON Web Token) based authentication and authorization.
* 🛍️ **Comprehensive Management:** Full **cart and order management** capabilities for users.
* 📦 **Real-Time Tracking:** Reliable and detailed **shipment tracking** functionality.
* 💾 **Data Integrity:** Seamless integration with **Entity Framework Core** for data access and modeling.
* 🏗️ **Sustainable Design:** A testable, maintainable, and scalable structure powered by **Clean Architecture**.
* 💡 **Integration Ready:** Designed for quick and easy integration with modern frontend applications, such as Angular.

---

## 🛠️ Tech Stack

The project is built upon a foundation of current and proven technologies.

| Layer | Technology | Description |
| :---: | :---: | :--- |
| **Backend** | **ASP.NET Core Web API** (.NET 8) | A powerful and performant environment for API development. |
| **ORM** | **Entity Framework Core** | The popular object-relational mapper (ORM) for the .NET ecosystem. |
| **Authentication** | **JWT (JSON Web Token)** | Secure and stateless session management. |
| **Architecture** | **Clean Architecture** | Layered design separating business logic from infrastructure. |
| **Language** | **C# 12** | A reliable and modern language enabling robust software development. |

---

## 🏗️ Architectural Structure (Clean Architecture)

The project is organized using a standard Clean Architecture pattern, ensuring clear separation of concerns:
TrackHub/ ┣ 📁 Domain/ → Core Business Rules, Entities, and Shared Interfaces. (The Innermost Layer) ┣ 📁 Application/ → Use Cases, Service Logic, and Data Transfer Objects (DTOs). ┣ 📁 Infrastructure/ → External Dependencies: EF Core Context, Data Access (Repositories), and External Services. ┣ 📁 API/ → Entry Point: API Controllers, Middleware, and Startup Configuration. ┗ 📄 README.md

---

## ⚙️ Quick Start

To run the TrackHub API locally, follow these simple steps:

1.  Navigate to the project source directory:
    ```bash
    cd src/TrackHub.API
    ```
2.  Run the project using `dotnet watch run` to start the API and automatically watch for code changes:
    ```bash
    dotnet watch run
    ```
3.  The API will be available at the default address:
    > **https://localhost:7115**

You are now ready to test the power of **TrackHub** and integrate it with your frontend application!
