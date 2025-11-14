# Design Patterns Documentation

This document explains the design patterns implemented in the Laptop Shop Management System.

## 1. Singleton Pattern

### Implementation: DatabaseConnection Class
**Location**: `Services/DatabaseConnection.cs`

### Purpose
Ensures that only one instance of the database connection manager exists throughout the application lifecycle.

### Code Example
```csharp
public sealed class DatabaseConnection
{
    private static DatabaseConnection? _instance;
    private static readonly object _lock = new object();
    
    private DatabaseConnection()
    {
        _connectionString = @"Server=localhost;Database=LaptopShopDB;...";
    }
    
    public static DatabaseConnection Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseConnection();
                    }
                }
            }
            return _instance;
        }
    }
}
```

### Benefits
- **Resource Management**: Single connection configuration across the application
- **Thread Safety**: Double-check locking prevents race conditions
- **Memory Efficiency**: Only one instance exists
- **Global Access Point**: Easy access from anywhere in the application

### Usage
```csharp
var connection = DatabaseConnection.Instance.GetConnection();
```

---

## 2. Repository Pattern

### Implementation: IRepository Interface and Entity Repositories
**Location**: `Repositories/`

### Purpose
Abstracts data access logic and provides a collection-like interface for accessing domain objects.

### Code Example
```csharp
public interface IRepository<T> where T : class
{
    List<T> GetAll();
    T? GetById(int id);
    bool Add(T entity);
    bool Update(T entity);
    bool Delete(int id);
}

public class LaptopRepository : IRepository<Laptop>
{
    public List<Laptop> GetAll() { /* Implementation */ }
    public Laptop? GetById(int id) { /* Implementation */ }
    // ... other methods
}
```

### Benefits
- **Separation of Concerns**: Data access logic separated from business logic
- **Testability**: Easy to mock repositories for unit testing
- **Maintainability**: Changes to data access don't affect business logic
- **Flexibility**: Easy to switch data sources (SQL Server, MySQL, etc.)
- **Reusability**: Generic interface can be used for any entity

### Implementations
- `UserRepository` - User data management
- `LaptopRepository` - Laptop inventory management
- `CustomerRepository` - Customer data management
- `OrderRepository` - Order processing

### Usage
```csharp
var laptopRepo = new LaptopRepository();
var laptops = laptopRepo.GetAll();
var laptop = laptopRepo.GetById(1);
laptopRepo.Add(newLaptop);
```

---

## 3. Factory Pattern

### Implementation: WindowFactory Class
**Location**: `Services/WindowFactory.cs`

### Purpose
Centralizes window creation logic and provides a consistent way to instantiate windows.

### Code Example
```csharp
public class WindowFactory
{
    public static Window CreateWindow(string windowType, User? user = null)
    {
        return windowType switch
        {
            "Login" => new Views.LoginWindow(),
            "Main" => new Views.MainWindow(user!),
            "LaptopManagement" => new Views.LaptopManagementWindow(user!),
            "OrderManagement" => new Views.OrderManagementWindow(user!),
            "CustomerManagement" => new Views.CustomerManagementWindow(user!),
            _ => throw new ArgumentException($"Unknown window type: {windowType}")
        };
    }
}
```

### Benefits
- **Encapsulation**: Window creation logic is centralized
- **Maintainability**: Easy to add new window types
- **Consistency**: All windows created through same interface
- **Flexibility**: Easy to modify window creation logic (e.g., add logging, validation)

### Usage
```csharp
var loginWindow = WindowFactory.CreateWindow("Login");
loginWindow.Show();

var mainWindow = WindowFactory.CreateWindow("Main", currentUser);
mainWindow.Show();
```

---

## 4. MVVM Pattern (Model-View-ViewModel)

### Implementation: Separation of Concerns Architecture
**Location**: Throughout the application

### Purpose
Separates user interface (View) from business logic (ViewModel) and data (Model).

### Structure

#### Models (`Models/`)
- Pure data classes
- No business logic
- Properties represent data structure
- Examples: `User`, `Laptop`, `Order`, `Customer`

```csharp
public class Laptop
{
    public int LaptopID { get; set; }
    public string Brand { get; set; }
    public decimal Price { get; set; }
    // ... other properties
}
```

#### Views (`Views/`)
- XAML files for UI definition
- Code-behind handles UI events
- No business logic
- Examples: `LoginWindow.xaml`, `MainWindow.xaml`

```xaml
<Window x:Class="LaptopShopApp.Views.LoginWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
    <Grid>
        <TextBox x:Name="TxtUsername" />
        <Button Content="Login" Click="BtnLogin_Click" />
    </Grid>
</Window>
```

#### ViewModels (Implied through Code-Behind and Services)
- Business logic
- Data validation
- State management
- Services like `AuthenticationService`

### Benefits
- **Separation of Concerns**: UI, logic, and data are independent
- **Testability**: Business logic can be tested without UI
- **Maintainability**: Changes to UI don't affect logic
- **Designer-Developer Workflow**: Designers work on XAML, developers on logic
- **Reusability**: ViewModels can be reused across different views

### Data Flow
1. **User Action** → View (XAML)
2. **Event Handler** → Code-Behind (View)
3. **Business Logic** → Services/Repositories
4. **Database Operation** → Repository
5. **Data Return** → Model
6. **UI Update** → View

---

## Design Pattern Benefits Summary

| Pattern | Primary Benefit | Secondary Benefit |
|---------|----------------|-------------------|
| Singleton | Resource Management | Thread Safety |
| Repository | Data Abstraction | Testability |
| Factory | Creation Encapsulation | Maintainability |
| MVVM | Separation of Concerns | Testability |

## Best Practices Applied

1. **Single Responsibility**: Each class has one clear purpose
2. **Open/Closed Principle**: Open for extension, closed for modification
3. **Dependency Inversion**: Depend on abstractions (IRepository)
4. **Don't Repeat Yourself (DRY)**: Reusable repository pattern
5. **Keep It Simple (KISS)**: Clear, understandable implementations

## Future Enhancements

Potential pattern additions:
- **Observer Pattern**: For real-time updates
- **Command Pattern**: For undo/redo functionality
- **Strategy Pattern**: For different authentication methods
- **Dependency Injection**: Using IoC containers like Microsoft.Extensions.DependencyInjection
