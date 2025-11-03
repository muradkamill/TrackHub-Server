# TrackHub Server

Excited to share the backend of **TrackHub**, a shipment and order management platform with marketplace capabilities. Built using **ASP.NET Core Web API** and following **Clean Architecture** principles, this project provides a solid foundation for scalable and maintainable full-stack development.

## Features

- **JWT Authentication** – Secure login and authorization for users and couriers.
- **Cart & Order Management** – Add products to cart, create orders, and manage order statuses.
- **Marketplace Functionality** – Users can list products for sale and purchase products.
- **Courier Management** – Apply as a courier, get approved, and fulfill deliveries.
- **Clean Architecture** – Layered architecture ready for integration with Angular frontend.
- **Scalable & Maintainable** – Designed for growth with proper separation of concerns.

## Tech Stack

- **Backend:** ASP.NET Core Web API
- **Architecture:** Clean Architecture
- **Authentication:** JWT
- **Database:** Microsoft SQL Server (or your choice)
- **Frontend-ready:** APIs ready to connect with Angular (or any frontend)

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- SQL Server (or alternative relational database)
- Visual Studio / VS Code

### Setup

1. Clone the repository:
```bash
git clone https://github.com/yourusername/TrackHub-Server.git
cd TrackHub-Server
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Configure the connection string in `appsettings.json`.

4. Apply migrations to create the database:
```bash
dotnet ef database update
```

5. Run the project:
```bash
dotnet run
```

The API will be available at `https://localhost:7115`.

## Next Steps

The backend is fully functional and ready for integration with a frontend. I am planning to develop the Angular frontend to provide a seamless user experience and interact with these APIs.

## Learning & Skills

This project has strengthened skills in:

- C# & .NET 8
- Clean Architecture & Layered Design
- API Development & Security (JWT)
- Full-Stack Development

## Contribution

This is currently a personal project. Contributions are welcome if you want to collaborate on improving or extending TrackHub.
