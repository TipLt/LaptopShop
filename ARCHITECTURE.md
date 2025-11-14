# System Architecture

## Overview

The Laptop Shop Management System follows a layered architecture with clear separation of concerns, implementing MVVM pattern principles and multiple design patterns.

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                     Presentation Layer                      │
│  ┌─────────────────────────────────────────────────────┐   │
│  │              Views (XAML + Code-Behind)             │   │
│  │  • LoginWindow                                      │   │
│  │  • MainWindow (Dashboard)                           │   │
│  │  • LaptopManagementWindow                           │   │
│  │  • OrderManagementWindow                            │   │
│  │  • CustomerManagementWindow                         │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                      Business Layer                         │
│  ┌─────────────────────────────────────────────────────┐   │
│  │                    Services                          │   │
│  │  • AuthenticationService (Login, Permissions)       │   │
│  │  • WindowFactory (Factory Pattern)                  │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    Data Access Layer                        │
│  ┌─────────────────────────────────────────────────────┐   │
│  │            Repositories (Repository Pattern)        │   │
│  │  • IRepository<T> (Generic Interface)               │   │
│  │  • UserRepository                                   │   │
│  │  • LaptopRepository                                 │   │
│  │  • CustomerRepository                               │   │
│  │  • OrderRepository                                  │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                   Database Connection                       │
│  ┌─────────────────────────────────────────────────────┐   │
│  │      DatabaseConnection (Singleton Pattern)         │   │
│  │  • Single instance                                  │   │
│  │  • Thread-safe                                      │   │
│  │  • Connection management                            │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                      Database Layer                         │
│  ┌─────────────────────────────────────────────────────┐   │
│  │               SQL Server Database                   │   │
│  │  • 9 Tables                                         │   │
│  │  • 3 Many-to-Many Relationships                     │   │
│  │  • Referential Integrity                            │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘

         ┌────────────────────────────────┐
         │         Models Layer           │
         │  • User                        │
         │  • Customer                    │
         │  • Laptop                      │
         │  • Order                       │
         │  • OrderDetail                 │
         │  • Category                    │
         │  • LaptopCategory              │
         │  • Supplier                    │
         │  • LaptopSupplier              │
         └────────────────────────────────┘
                (Used across all layers)
```

## Component Interaction Flow

### 1. User Login Flow

```
User enters credentials
        │
        ▼
[LoginWindow.xaml.cs]
        │
        ▼
[AuthenticationService.Login()]
        │
        ▼
[UserRepository.Authenticate()]
        │
        ▼
[DatabaseConnection.GetConnection()]
        │
        ▼
[SQL Server: SELECT FROM Users]
        │
        ▼
[User Model returned]
        │
        ▼
[WindowFactory.CreateWindow("Main", user)]
        │
        ▼
[MainWindow displayed]
```

### 2. Laptop CRUD Flow

```
User clicks "Add Laptop"
        │
        ▼
[LaptopManagementWindow - BtnAdd_Click]
        │
        ▼
[Check permissions: AuthenticationService.HasPermission()]
        │
        ├─ No Permission → Show error message
        │
        └─ Has Permission
                │
                ▼
        User fills form
                │
                ▼
        Click "Save"
                │
                ▼
        [Validate input data]
                │
                ▼
        [Create Laptop model]
                │
                ▼
        [LaptopRepository.Add(laptop)]
                │
                ▼
        [DatabaseConnection.GetConnection()]
                │
                ▼
        [SQL: INSERT INTO Laptops]
                │
                ▼
        [Success message]
                │
                ▼
        [Refresh grid]
```

### 3. Order Creation Flow

```
Sales user creates order
        │
        ▼
[OrderManagementWindow - BtnAdd_Click]
        │
        ▼
[Check permissions]
        │
        ▼
[User enters order data]
        │
        ▼
[OrderRepository.Add(order)]
        │
        ├─ INSERT INTO Orders
        │
        └─ Get new OrderID
                │
                ▼
        [OrderRepository.AddOrderDetail()]
                │
                ▼
        [INSERT INTO OrderDetails]
                │
                ▼
        [Success confirmation]
```

## Design Pattern Implementation

### 1. Singleton Pattern

```
Application starts
        │
        ▼
First database operation needed
        │
        ▼
DatabaseConnection.Instance called
        │
        ├─ Instance exists? → Return existing
        │
        └─ Instance null?
                │
                ▼
        Enter lock block
                │
                ▼
        Double-check null
                │
                ▼
        Create new instance
                │
                ▼
        Return instance
```

**Benefits**:
- Single connection configuration
- Thread-safe access
- Memory efficient

### 2. Repository Pattern

```
Any window needs data
        │
        ▼
Create repository instance
        │
        ▼
Call repository method
        │
        ├─ GetAll() → SELECT * FROM Table
        ├─ GetById(id) → SELECT * FROM Table WHERE ID = @ID
        ├─ Add(entity) → INSERT INTO Table
        ├─ Update(entity) → UPDATE Table
        └─ Delete(id) → DELETE FROM Table
                │
                ▼
        Use DatabaseConnection singleton
                │
                ▼
        Execute SQL with parameters
                │
                ▼
        Return result
```

**Benefits**:
- Abstracted data access
- Testable code
- Reusable queries

### 3. Factory Pattern

```
Need to open a window
        │
        ▼
WindowFactory.CreateWindow(type, user)
        │
        ├─ type == "Login" → new LoginWindow()
        ├─ type == "Main" → new MainWindow(user)
        ├─ type == "LaptopManagement" → new LaptopManagementWindow(user)
        ├─ type == "OrderManagement" → new OrderManagementWindow(user)
        └─ type == "CustomerManagement" → new CustomerManagementWindow(user)
                │
                ▼
        Return window instance
                │
                ▼
        Call .Show() on window
```

**Benefits**:
- Centralized creation logic
- Easy to add new windows
- Consistent instantiation

### 4. MVVM Pattern

```
┌──────────┐         ┌────────────┐         ┌─────────┐
│  Model   │◄────────│ ViewModel  │◄────────│  View   │
│          │         │  (Service) │         │ (XAML)  │
└──────────┘         └────────────┘         └─────────┘
    │                      │                      │
    │                      │                      │
Data entities      Business logic           User interface
Properties only    Validation              Event handlers
No logic           Repositories            Data binding
                   State management
```

**Implementation**:
- **Models**: Pure data classes (User, Laptop, etc.)
- **Views**: XAML files with code-behind
- **ViewModels**: Services (AuthenticationService, WindowFactory)

## Data Flow Patterns

### Read Operation (GET)

```
[View] → [Event Handler] → [Check Permission] → [Repository]
                                                      │
                                                      ▼
[Display] ←─ [Map to Model] ←─ [SqlDataReader] ←─ [Execute Query]
```

### Write Operation (POST/PUT/DELETE)

```
[View] → [Event Handler] → [Check Permission] → [Validate Input]
                                                      │
                                                      ▼
[Feedback] ←─ [Success/Error] ←─ [Execute] ←─ [Repository]
```

## Security Architecture

```
User attempts action
        │
        ▼
AuthenticationService.HasPermission(action, entity)
        │
        ├─ CurrentUser null? → Deny
        │
        └─ Check role permissions
                │
                ├─ Admin? → Allow all
                │
                ├─ Manager? → Check entity rules
                │
                ├─ Sales? → Check entity rules
                │
                └─ Warehouse? → Check entity rules
                        │
                        ▼
                Return boolean
                        │
                        ├─ true → Proceed
                        │
                        └─ false → Show permission denied message
```

## Role-Based Access Matrix

```
┌──────────────┬───────┬─────────┬───────┬───────────┐
│   Entity     │ Admin │ Manager │ Sales │ Warehouse │
├──────────────┼───────┼─────────┼───────┼───────────┤
│ Laptops      │ CRUD  │  CRUD   │   R   │    RU     │
├──────────────┼───────┼─────────┼───────┼───────────┤
│ Orders       │ CRUD  │    R    │  CR   │     -     │
├──────────────┼───────┼─────────┼───────┼───────────┤
│ Customers    │ CRUD  │    R    │   R   │     -     │
├──────────────┼───────┼─────────┼───────┼───────────┤
│ Users        │ CRUD  │    -    │   -   │     -     │
└──────────────┴───────┴─────────┴───────┴───────────┘

Legend: C=Create, R=Read, U=Update, D=Delete, -=No Access
```

## Database Architecture

### Entity Relationships

```
                    ┌─────────────┐
                    │    Users    │
                    │  (AuthN)    │
                    └─────────────┘

┌─────────────┐           ┌─────────────┐           ┌─────────────┐
│  Customers  │───────────│   Orders    │───────┐   │   Laptops   │
└─────────────┘     1:N   └─────────────┘       │   └─────────────┘
                                │                │         │
                                │ 1:N            │ N:M     │ N:M
                                ▼                ▼         ▼
                    ┌──────────────────┐   ┌──────────────────┐
                    │  OrderDetails    │   │ LaptopCategories │
                    │  (N:M Junction)  │   │  (N:M Junction)  │
                    └──────────────────┘   └──────────────────┘
                                                     │
                                                     │ N:1
                                                     ▼
┌─────────────┐                              ┌─────────────┐
│  Suppliers  │                              │ Categories  │
└─────────────┘                              └─────────────┘
      │                                           
      │ N:M                                      
      ▼                                          
┌──────────────────┐
│ LaptopSuppliers  │
│  (N:M Junction)  │
└──────────────────┘
```

### Connection Management

```
Application Startup
        │
        ▼
DatabaseConnection instance created (Singleton)
        │
        ▼
Connection string configured
        │
        ▼
Multiple operations throughout app lifecycle
        │
        ├─ Operation 1: GetConnection() → SqlConnection
        ├─ Operation 2: GetConnection() → SqlConnection
        ├─ Operation 3: GetConnection() → SqlConnection
        └─ ... more operations
                │
                ▼
        Each SqlConnection disposed after use
                │
                ▼
        Singleton instance persists
                │
                ▼
        Application Exit → Cleanup
```

## Scalability Considerations

### Current Architecture
- Single-tier desktop application
- Direct database connection
- Client-side business logic

### Future Scaling Options

```
Current:
[WPF Client] ──────► [SQL Server]

Potential Future:
[WPF Client] ──► [REST API] ──► [Business Layer] ──► [SQL Server]
                                      │
[Web Client] ────►                   │
                                      │
[Mobile App] ────►                   │
```

## Performance Optimization

### Current Optimizations
1. **Singleton Pattern**: Reuses connection configuration
2. **Using Statements**: Proper resource disposal
3. **Parameterized Queries**: SQL injection prevention + query plan caching
4. **Indexed Columns**: Primary and foreign keys auto-indexed

### Potential Optimizations
1. Connection pooling (built into ADO.NET)
2. Async/await for database operations
3. Caching frequently accessed data
4. Batch operations for bulk inserts
5. Stored procedures for complex queries

## Error Handling Strategy

```
Operation Attempted
        │
        ▼
    Try Block
        │
        ├─ Success → Continue
        │
        └─ Exception
                │
                ▼
            Catch Block
                │
                ├─ Log error
                ├─ Show user-friendly message
                └─ Return to safe state
                        │
                        ▼
                    Finally Block
                        │
                        └─ Cleanup resources
```

## Deployment Architecture

```
┌─────────────────────────────────────────┐
│         Client Workstation              │
│  ┌────────────────────────────────┐    │
│  │   LaptopShopApp.exe            │    │
│  │   .NET 8.0 Runtime             │    │
│  │   Windows 10/11                │    │
│  └────────────────────────────────┘    │
└─────────────────────────────────────────┘
                │
                │ TCP/IP
                │ (Default: 1433)
                ▼
┌─────────────────────────────────────────┐
│         Database Server                 │
│  ┌────────────────────────────────┐    │
│  │   SQL Server                   │    │
│  │   LaptopShopDB                 │    │
│  │   Port 1433 (default)          │    │
│  └────────────────────────────────┘    │
└─────────────────────────────────────────┘
```

## Technology Stack Layers

```
┌─────────────────────────────────────────┐
│        User Interface Layer             │
│  • WPF (XAML)                           │
│  • Material Design Colors               │
│  • DataGrid, TextBox, Button, etc.      │
└─────────────────────────────────────────┘
                │
┌─────────────────────────────────────────┐
│      Application Logic Layer            │
│  • C# 12                                │
│  • .NET 8.0                             │
│  • Event Handlers                       │
└─────────────────────────────────────────┘
                │
┌─────────────────────────────────────────┐
│       Business Logic Layer              │
│  • Services                             │
│  • Validation                           │
│  • Authorization                        │
└─────────────────────────────────────────┘
                │
┌─────────────────────────────────────────┐
│      Data Access Layer                  │
│  • ADO.NET                              │
│  • Repository Pattern                   │
│  • SqlClient                            │
└─────────────────────────────────────────┘
                │
┌─────────────────────────────────────────┐
│         Database Layer                  │
│  • SQL Server                           │
│  • T-SQL                                │
│  • Relational Model                     │
└─────────────────────────────────────────┘
```

## Summary

The Laptop Shop Management System implements a clean, layered architecture with:

- **Clear separation of concerns** across layers
- **Multiple design patterns** for maintainability
- **Role-based security** at the business layer
- **Repository pattern** for data abstraction
- **Singleton pattern** for resource management
- **Factory pattern** for object creation
- **MVVM principles** for UI separation

This architecture ensures the system is:
- ✅ Maintainable
- ✅ Testable
- ✅ Scalable
- ✅ Secure
- ✅ Well-organized
