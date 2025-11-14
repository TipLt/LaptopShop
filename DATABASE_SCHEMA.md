# Database Schema Documentation

## Overview
The Laptop Shop database consists of 9 tables with 3 many-to-many relationships, designed to manage a complete laptop retail operation.

## Entity Relationship Diagram (ERD)

```
┌─────────────┐
│    Users    │
└─────────────┘

┌─────────────┐         ┌──────────────┐         ┌─────────────┐
│  Customers  │────────>│    Orders    │<────┐   │   Laptops   │
└─────────────┘         └──────────────┘     │   └─────────────┘
                              │              │         │
                              │              │         │
                              ▼              │         ▼
                        ┌──────────────┐     │   ┌──────────────────┐
                        │ OrderDetails │─────┘   │ LaptopCategories │
                        └──────────────┘         └──────────────────┘
                           (N-N Table)                  │
                                                        │
┌──────────────┐                                       ▼
│  Suppliers   │                              ┌──────────────┐
└──────────────┘                              │  Categories  │
      │                                       └──────────────┘
      │
      ▼
┌──────────────────┐
│ LaptopSuppliers  │
└──────────────────┘
   (N-N Table)
```

## Table Definitions

### 1. Users Table
**Purpose**: Store user accounts with role-based access control

| Column       | Type          | Constraints                    | Description                        |
|--------------|---------------|--------------------------------|------------------------------------|
| UserID       | INT           | PRIMARY KEY, IDENTITY(1,1)     | Auto-incrementing user ID          |
| Username     | NVARCHAR(50)  | NOT NULL, UNIQUE               | Unique login username              |
| Password     | NVARCHAR(255) | NOT NULL                       | User password (plain text for demo)|
| Role         | NVARCHAR(20)  | NOT NULL, CHECK                | Admin, Sales, Manager, Warehouse   |
| FullName     | NVARCHAR(100) | NOT NULL                       | User's full name                   |
| Email        | NVARCHAR(100) |                                | Contact email                      |
| CreatedDate  | DATETIME      | DEFAULT GETDATE()              | Account creation date              |
| IsActive     | BIT           | DEFAULT 1                      | Soft delete flag                   |

**Sample Data**:
```sql
admin / admin123 (Admin)
sales1 / sales123 (Sales)
manager1 / manager123 (Manager)
warehouse1 / warehouse123 (Warehouse)
```

---

### 2. Customers Table
**Purpose**: Store customer information

| Column       | Type          | Constraints                | Description              |
|--------------|---------------|----------------------------|--------------------------|
| CustomerID   | INT           | PRIMARY KEY, IDENTITY(1,1) | Auto-incrementing ID     |
| CustomerName | NVARCHAR(100) | NOT NULL                   | Customer's full name     |
| Email        | NVARCHAR(100) |                            | Contact email            |
| Phone        | NVARCHAR(20)  |                            | Phone number             |
| Address      | NVARCHAR(255) |                            | Physical address         |
| CreatedDate  | DATETIME      | DEFAULT GETDATE()          | Registration date        |

---

### 3. Laptops Table
**Purpose**: Store laptop inventory information

| Column      | Type           | Constraints                | Description                    |
|-------------|----------------|----------------------------|--------------------------------|
| LaptopID    | INT            | PRIMARY KEY, IDENTITY(1,1) | Auto-incrementing laptop ID    |
| Brand       | NVARCHAR(50)   | NOT NULL                   | Manufacturer (Dell, HP, etc.)  |
| Model       | NVARCHAR(100)  | NOT NULL                   | Product model name             |
| Processor   | NVARCHAR(100)  |                            | CPU specification              |
| RAM         | NVARCHAR(50)   |                            | Memory specification           |
| Storage     | NVARCHAR(50)   |                            | Storage specification          |
| GPU         | NVARCHAR(100)  |                            | Graphics card                  |
| Price       | DECIMAL(18,2)  | NOT NULL                   | Selling price in VND           |
| Stock       | INT            | DEFAULT 0                  | Available quantity             |
| Description | NVARCHAR(500)  |                            | Product description            |

---

### 4. Orders Table
**Purpose**: Store order header information

| Column      | Type           | Constraints                | Description                    |
|-------------|----------------|----------------------------|--------------------------------|
| OrderID     | INT            | PRIMARY KEY, IDENTITY(1,1) | Auto-incrementing order ID     |
| CustomerID  | INT            | FOREIGN KEY → Customers    | Reference to customer          |
| OrderDate   | DATETIME       | DEFAULT GETDATE()          | Order creation timestamp       |
| TotalAmount | DECIMAL(18,2)  | NOT NULL                   | Total order value              |
| Status      | NVARCHAR(20)   | DEFAULT 'Pending', CHECK   | Order status                   |
| Notes       | NVARCHAR(500)  |                            | Additional notes               |

**Status Values**: Pending, Processing, Completed, Cancelled

---

### 5. OrderDetails Table (N-N: Orders ↔ Laptops)
**Purpose**: Junction table linking orders to laptops with quantities

| Column        | Type           | Constraints                | Description                    |
|---------------|----------------|----------------------------|--------------------------------|
| OrderDetailID | INT            | PRIMARY KEY, IDENTITY(1,1) | Auto-incrementing detail ID    |
| OrderID       | INT            | FOREIGN KEY → Orders       | Reference to order             |
| LaptopID      | INT            | FOREIGN KEY → Laptops      | Reference to laptop            |
| Quantity      | INT            | NOT NULL                   | Number of units                |
| UnitPrice     | DECIMAL(18,2)  | NOT NULL                   | Price per unit at order time   |

**Many-to-Many Relationship**: 
- One Order can have many Laptops
- One Laptop can be in many Orders

---

### 6. Categories Table
**Purpose**: Store laptop categories for classification

| Column        | Type          | Constraints                | Description              |
|---------------|---------------|----------------------------|--------------------------|
| CategoryID    | INT           | PRIMARY KEY, IDENTITY(1,1) | Auto-incrementing ID     |
| CategoryName  | NVARCHAR(50)  | NOT NULL, UNIQUE           | Category name            |
| Description   | NVARCHAR(255) |                            | Category description     |

**Sample Categories**: Gaming, Business, Ultrabook, Workstation

---

### 7. LaptopCategories Table (N-N: Laptops ↔ Categories)
**Purpose**: Junction table for laptop categorization

| Column     | Type | Constraints                   | Description           |
|------------|------|-------------------------------|-----------------------|
| LaptopID   | INT  | FOREIGN KEY → Laptops         | Reference to laptop   |
| CategoryID | INT  | FOREIGN KEY → Categories      | Reference to category |

**Composite Primary Key**: (LaptopID, CategoryID)

**Many-to-Many Relationship**: 
- One Laptop can belong to many Categories
- One Category can have many Laptops

---

### 8. Suppliers Table
**Purpose**: Store supplier information

| Column        | Type          | Constraints                | Description              |
|---------------|---------------|----------------------------|--------------------------|
| SupplierID    | INT           | PRIMARY KEY, IDENTITY(1,1) | Auto-incrementing ID     |
| SupplierName  | NVARCHAR(100) | NOT NULL                   | Supplier company name    |
| ContactPerson | NVARCHAR(100) |                            | Contact person name      |
| Email         | NVARCHAR(100) |                            | Contact email            |
| Phone         | NVARCHAR(20)  |                            | Contact phone            |
| Address       | NVARCHAR(255) |                            | Physical address         |

---

### 9. LaptopSuppliers Table (N-N: Laptops ↔ Suppliers)
**Purpose**: Junction table tracking laptop suppliers with pricing

| Column      | Type          | Constraints               | Description                 |
|-------------|---------------|---------------------------|-----------------------------|
| LaptopID    | INT           | FOREIGN KEY → Laptops     | Reference to laptop         |
| SupplierID  | INT           | FOREIGN KEY → Suppliers   | Reference to supplier       |
| SupplyDate  | DATETIME      | DEFAULT GETDATE()         | Date of supply              |
| SupplyPrice | DECIMAL(18,2) |                           | Purchase price from supplier|

**Composite Primary Key**: (LaptopID, SupplierID)

**Many-to-Many Relationship**: 
- One Laptop can have many Suppliers
- One Supplier can supply many Laptops

---

## Relationships Summary

### One-to-Many Relationships
1. **Customers → Orders**: One customer can have many orders
2. **Orders → OrderDetails**: One order can have many detail lines
3. **Laptops → OrderDetails**: One laptop can appear in many order details
4. **Categories → LaptopCategories**: One category can have many laptop links
5. **Suppliers → LaptopSuppliers**: One supplier can supply many laptops

### Many-to-Many Relationships
1. **Orders ↔ Laptops** (via OrderDetails)
   - Represents: Which laptops were ordered in which orders
   - Additional data: Quantity, UnitPrice

2. **Laptops ↔ Categories** (via LaptopCategories)
   - Represents: Which categories each laptop belongs to
   - Example: Dell XPS 15 → [Business, Ultrabook]

3. **Laptops ↔ Suppliers** (via LaptopSuppliers)
   - Represents: Which suppliers provide which laptops
   - Additional data: SupplyDate, SupplyPrice

## Database Features

### Constraints
- **Primary Keys**: All tables have auto-incrementing primary keys
- **Foreign Keys**: Enforce referential integrity
- **CHECK Constraints**: 
  - User.Role ∈ {Admin, Sales, Manager, Warehouse}
  - Order.Status ∈ {Pending, Processing, Completed, Cancelled}
- **UNIQUE Constraints**: Username, CategoryName
- **NOT NULL**: Essential fields cannot be null

### Default Values
- **Timestamps**: CreatedDate, OrderDate → GETDATE()
- **Status**: Order.Status → 'Pending'
- **Flags**: User.IsActive → 1
- **Counters**: Laptop.Stock → 0

### Indexes
Implicit indexes on:
- All Primary Keys
- All Foreign Keys
- UNIQUE constraints (Username, CategoryName)

### Data Integrity
- **Cascading**: Not implemented (manual handling required)
- **Soft Delete**: Users table uses IsActive flag
- **Audit Trail**: CreatedDate fields track creation times

## Sample Data Relationships

### Example 1: Order with Multiple Laptops
```
Order #1 (Nguyen Van A)
├── Dell XPS 15 (Qty: 1, Price: 35,000,000 VND)
└── Total: 35,000,000 VND
```

### Example 2: Laptop with Multiple Categories
```
Dell XPS 15
├── Category: Business
└── Category: Ultrabook
```

### Example 3: Laptop with Multiple Suppliers
```
Dell XPS 15
├── Supplier: Dell Vietnam (Price: 30,000,000 VND)
└── (Can add more suppliers)
```

## Query Examples

### Get All Orders for a Customer
```sql
SELECT o.*, od.*, l.Brand, l.Model
FROM Orders o
JOIN OrderDetails od ON o.OrderID = od.OrderID
JOIN Laptops l ON od.LaptopID = l.LaptopID
WHERE o.CustomerID = 1;
```

### Get All Categories for a Laptop
```sql
SELECT l.Brand, l.Model, c.CategoryName
FROM Laptops l
JOIN LaptopCategories lc ON l.LaptopID = lc.LaptopID
JOIN Categories c ON lc.CategoryID = c.CategoryID
WHERE l.LaptopID = 1;
```

### Get All Suppliers for a Laptop
```sql
SELECT l.Brand, l.Model, s.SupplierName, ls.SupplyPrice
FROM Laptops l
JOIN LaptopSuppliers ls ON l.LaptopID = ls.LaptopID
JOIN Suppliers s ON ls.SupplierID = s.SupplierID
WHERE l.LaptopID = 1;
```

## Performance Considerations

1. **Indexes**: Primary and foreign keys are automatically indexed
2. **Normalization**: 3NF normalized to reduce redundancy
3. **Query Optimization**: Use appropriate JOINs for many-to-many queries
4. **Connection Pooling**: Managed by Singleton pattern in application

## Future Enhancements

Potential schema improvements:
- Add audit tables for tracking changes
- Implement soft delete for all tables
- Add full-text search indexes
- Partition large tables by date
- Add check constraints for price ranges
- Add triggers for stock management
