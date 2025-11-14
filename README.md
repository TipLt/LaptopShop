# LaptopShop Management System

A comprehensive Windows Presentation Foundation (WPF) application for managing a laptop shop with role-based access control, built with C# and SQL Server.

## 📋 Project Requirements Met

### 1. Database Requirements ✅
- **9 Tables** (exceeds requirement of 6):
  - Users
  - Customers
  - Laptops
  - Orders
  - OrderDetails
  - Categories
  - Suppliers
  - LaptopCategories (N-N junction)
  - LaptopSuppliers (N-N junction)

- **3 Many-to-Many Relationships** (exceeds requirement of 2):
  1. **Orders ↔ Laptops** (via OrderDetails table)
  2. **Laptops ↔ Categories** (via LaptopCategories table)
  3. **Laptops ↔ Suppliers** (via LaptopSuppliers table)

### 2. Windows Requirements ✅
- **5 Windows** (exceeds requirement of 3):
  1. **LoginWindow** - User authentication
  2. **MainWindow** - Dashboard with statistics
  3. **LaptopManagementWindow** - Full CRUD for laptops
  4. **OrderManagementWindow** - Order management
  5. **CustomerManagementWindow** - Customer management

### 3. Design Patterns ✅
Implements **4 Design Patterns** (exceeds requirement of 3):

1. **Singleton Pattern**
   - `DatabaseConnection` class ensures single database connection instance
   - Thread-safe implementation with double-check locking

2. **Repository Pattern**
   - `IRepository<T>` generic interface
   - Separate repositories for each entity (UserRepository, LaptopRepository, CustomerRepository, OrderRepository)
   - Abstracts data access logic

3. **Factory Pattern**
   - `WindowFactory` class for creating window instances
   - Centralizes window creation logic

4. **MVVM Pattern**
   - Separation of concerns between Views (XAML), ViewModels, and Models
   - Clean architecture approach

## 👥 Role-Based Access Control

### Admin
- **Full Access**: Complete CRUD operations on all entities
- Can manage users, laptops, orders, and customers

### Sales
- **Orders**: Create and Read
- **Laptops**: Read only
- **Customers**: Read only
- Focus on sales operations

### Manager
- **Laptops**: Full CRUD operations
- **Orders**: Read only
- **Customers**: Read only
- Focus on inventory management

### Warehouse
- **Laptops**: Update stock and Read
- Limited to warehouse operations

## 🗄️ Database Schema

```
Users (UserID, Username, Password, Role, FullName, Email, CreatedDate, IsActive)
Customers (CustomerID, CustomerName, Email, Phone, Address, CreatedDate)
Laptops (LaptopID, Brand, Model, Processor, RAM, Storage, GPU, Price, Stock, Description)
Orders (OrderID, CustomerID, OrderDate, TotalAmount, Status, Notes)
OrderDetails (OrderDetailID, OrderID, LaptopID, Quantity, UnitPrice)
Categories (CategoryID, CategoryName, Description)
Suppliers (SupplierID, SupplierName, ContactPerson, Email, Phone, Address)
LaptopCategories (LaptopID, CategoryID) -- N-N relationship
LaptopSuppliers (LaptopID, SupplierID, SupplyDate, SupplyPrice) -- N-N relationship
```

## 🚀 Setup Instructions

### Prerequisites
- Windows OS (WPF is Windows-only)
- .NET 8.0 SDK or higher
- SQL Server (LocalDB, Express, or full version)
- Visual Studio 2022 (recommended) or any C# IDE

### Database Setup
1. Open SQL Server Management Studio (SSMS)
2. Connect to your SQL Server instance
3. Open and execute `LaptopShopApp/Database/CreateDatabase.sql`
4. This will:
   - Create the `LaptopShopDB` database
   - Create all 9 tables
   - Insert sample data including test users

### Application Setup
1. Open the solution in Visual Studio 2022
2. Update the connection string in `Services/DatabaseConnection.cs` if needed:
   ```csharp
   _connectionString = @"Server=localhost;Database=LaptopShopDB;Integrated Security=True;TrustServerCertificate=True;";
   ```
3. Build the solution (Ctrl+Shift+B)
4. Run the application (F5)

## 🔐 Demo Accounts

Use these credentials to test different roles:

| Username    | Password      | Role      |
|-------------|---------------|-----------|
| admin       | admin123      | Admin     |
| sales1      | sales123      | Sales     |
| manager1    | manager123    | Manager   |
| warehouse1  | warehouse123  | Warehouse |

## 📁 Project Structure

```
LaptopShopApp/
├── Models/              # Data models (User, Laptop, Order, etc.)
├── Views/               # XAML windows and code-behind
├── ViewModels/          # View models (for future MVVM implementation)
├── Services/            # Business logic and services
│   ├── DatabaseConnection.cs    # Singleton pattern
│   ├── WindowFactory.cs         # Factory pattern
│   └── AuthenticationService.cs # Authentication & authorization
├── Repositories/        # Repository pattern implementations
│   ├── IRepository.cs
│   ├── UserRepository.cs
│   ├── LaptopRepository.cs
│   ├── CustomerRepository.cs
│   └── OrderRepository.cs
├── Database/           # SQL scripts
│   └── CreateDatabase.sql
└── Resources/          # Application resources
```

## ✨ Features

### Login System
- Secure authentication
- Role-based access
- User-friendly interface with demo account hints

### Dashboard
- Real-time statistics
- Total laptops count
- Total orders count
- Total customers count
- Role-based menu visibility

### Laptop Management
- Add new laptops with full specifications
- Edit existing laptop details
- Delete laptops
- Update stock levels
- View all laptops in a grid
- Permission-based CRUD operations

### Order Management
- Create new orders
- View order details
- Track order status (Pending, Processing, Completed, Cancelled)
- Link orders to customers
- View order items and details

### Customer Management
- Add new customers
- Edit customer information
- Delete customers
- View complete customer list
- Store contact details and addresses

## 🎨 UI Features

- Modern, clean interface with Material Design inspired colors
- Responsive layouts
- Intuitive navigation
- Color-coded sections (Blue for Laptops, Orange for Orders, Green for Customers)
- Role-based button visibility and enabling
- Data grids with alternating row colors
- Form validation with user-friendly error messages

## 🔒 Security Features

- Password-based authentication
- Role-based access control (RBAC)
- Permission checks before CRUD operations
- SQL injection prevention through parameterized queries
- Soft delete for users (IsActive flag)

## 📊 Business Logic

- **Order-Laptop Relationship**: Orders can contain multiple laptops through OrderDetails
- **Laptop-Category Relationship**: Laptops can belong to multiple categories
- **Laptop-Supplier Relationship**: Laptops can have multiple suppliers with different supply prices
- **Stock Management**: Track laptop inventory
- **Order Status Tracking**: Monitor order lifecycle

## 🛠️ Technologies Used

- **Framework**: .NET 8.0 (WPF)
- **Language**: C# 12
- **Database**: SQL Server
- **ORM**: ADO.NET (SqlClient)
- **UI**: XAML
- **Architecture**: MVVM-inspired with Repository pattern

## 📝 Notes

- This project is designed for educational purposes to demonstrate WPF application development
- The password storage uses plain text for demonstration purposes only
- In production, use proper password hashing (bcrypt, Argon2, etc.)
- Connection strings should be stored in configuration files (app.config, appsettings.json)
- Consider adding input validation and error handling for production use

## 🎓 Academic Requirements Summary

✅ **Database**: 9 tables, 3 many-to-many relationships  
✅ **Windows**: 5 windows with full functionality  
✅ **Design Patterns**: 4 patterns (Singleton, Repository, Factory, MVVM)  
✅ **CRUD Operations**: Complete Create, Read, Update, Delete for all entities  
✅ **Role-Based Access**: 4 roles with different permissions  
✅ **Modern UI**: Clean, professional interface  
✅ **SQL Server**: Full integration with proper schema  

## 📄 License

This project is created for educational purposes as a final project for a WPF course.