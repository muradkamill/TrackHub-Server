# 📦 Online Market – Order & Shipping Management System

A fully featured backend system for managing online orders, shopping carts, product inventory, shipping operations, and admin processes built with **Clean Architecture** and **CQRS** pattern.

---

## 🚀 Features

### 🔐 Authentication & Authorization
- User registration & login (JWT-based)
- Password hashing
- Role-based authorization (User / Admin / Courier)

### 👤 User Management
- View & update user profile
- Multiple address support
- Profile photo upload (optional)

### 🛒 Product Management
- Add / update / delete products (CRUD)
- Category → Subcategory structure
- Inventory / stock tracking
- Product image upload (local storage)

### 🧺 Cart Module
- Add or remove items from cart
- Update item quantities
- Clear cart
- Convert cart to order

### 📦 Order Management
- Create orders from cart
- Track order lifecycle: `Pending → Approved → Shipped → Delivered`
- Order history
- Order cancellation

### 🚚 Shipping & Delivery Module
- Generate shipment tracking code
- Courier login & delivery confirmation
- Delivery photo or signature (optional)
- Delivery timestamp logging

### 🛠 Admin Panel API
- List all users
- Manage products
- Manage orders
- Stock alert system (notify when stock < X)
- Audit log access

### 📝 Audit Logging
- Tracks who performed what action, and when
- Logs IP address, endpoint, timestamp, and action details

### 🔔 Real-time Notifications with SignalR
- Real-time order status updates
- Notify users when order status changes
- Notify when shipment is dispatched
- Live notifications to admins and users

### 💳 Payment Simulation (Optional)
- Fake payment workflow (no real transactions)
- "Payment successful" → triggers order creation

### 📊 Reporting & Analytics (Optional)
- Daily/weekly/monthly sales reports
- Best-selling products
- User activity and order statistics

---

## 🧰 Technologies Used

- **.NET 9**
- **Entity Framework Core**
- **SQL Server / PostgreSQL**
- **JWT Authentication**
- **MediatR** (CQRS Pattern)
- **Clean Architecture**
- **SignalR** (Real-time Communication)
- **OData**
- **FluentValidation**
- **Docker**
- **OpenAPI + Scalar API Documentation**

---

## 📁 Project Structure (Clean Architecture)
```
/src
  /Core
    /Domain           # Entities, Value Objects, Domain Events
    /Application      # Use Cases, CQRS (Commands/Queries), Interfaces
  /Infrastructure     # EF Core, External Services, Repositories
  /WebAPI             # Controllers, SignalR Hubs, Middleware
```

---

## ▶️ Getting Started

### 1. Install dependencies
```bash
dotnet restore
```

### 2. Apply database migrations
```bash
dotnet ef database update
```

### 3. Run the API
```bash
dotnet run --project WebAPI
```

---

## 🔑 Environment Variables (appsettings.json)
```json
{
  "Jwt": {
    "Key": "your-secret-key-here",
    "Issuer": "OnlineMarketAPI",
    "Audience": "OnlineMarketClient"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=OnlineMarketDB;User Id=sa;Password=YourPassword;"
  }
}
```

---

## 📘 API Documentation (OpenAPI + Scalar)

This project uses **OpenAPI 3.0** and **Scalar** for modern, clean API documentation.

Once the API is running, you can access the Scalar UI at:
```
/scalar/v1
```

**Example:**
```
https://localhost:5001/scalar/v1
```

---

## 🔄 CQRS with MediatR

This project implements **CQRS (Command Query Responsibility Segregation)** pattern using **MediatR**.

### Example Command:
```csharp
public class CreateOrderCommand : IRequest<OrderDto>
{
    public int UserId { get; set; }
    public List<OrderItemDto> Items { get; set; }
}
```

### Example Query:
```csharp
public class GetOrderByIdQuery : IRequest<OrderDto>
{
    public int OrderId { get; set; }
}
```

---

## 📡 Real-time Communication with SignalR

Real-time notifications are implemented using **SignalR** for instant updates:

- Order status changes
- New order notifications for admins
- Shipment tracking updates
- Stock alerts

### SignalR Hub Endpoint:
```
/hubs/notifications
```

---

## 🤝 Contributing

Contributions, issues, and pull requests are always welcome!

---

## 📄 License

This project is licensed under the MIT License.

---

## 👨‍💻 Author

**Murad Kamil**  
[GitHub](https://github.com/muradkamill) | [LinkedIn](https://www.linkedin.com/in/muradkamil/)
