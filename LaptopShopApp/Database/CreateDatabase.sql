-- Create Database
CREATE DATABASE LaptopShopDB;
GO

USE LaptopShopDB;
GO

-- Table 1: Users
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Admin', 'Sales', 'Manager', 'Warehouse')),
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);

-- Table 2: Customers
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    CustomerName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Address NVARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- Table 3: Categories
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255)
);

-- Table 4: Suppliers
CREATE TABLE Suppliers (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    SupplierName NVARCHAR(100) NOT NULL,
    ContactPerson NVARCHAR(100),
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Address NVARCHAR(255)
);

-- Table 5: Laptops
CREATE TABLE Laptops (
    LaptopID INT PRIMARY KEY IDENTITY(1,1),
    Brand NVARCHAR(50) NOT NULL,
    Model NVARCHAR(100) NOT NULL,
    Processor NVARCHAR(100),
    RAM NVARCHAR(50),
    Storage NVARCHAR(50),
    GPU NVARCHAR(100),
    Price DECIMAL(18,2) NOT NULL,
    Stock INT DEFAULT 0,
    Description NVARCHAR(500)
);

-- Table 6: Orders
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID),
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Processing', 'Completed', 'Cancelled')),
    Notes NVARCHAR(500)
);

-- Table 7: OrderDetails (Junction table - N-N relationship between Orders and Laptops)
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
    LaptopID INT FOREIGN KEY REFERENCES Laptops(LaptopID),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL
);

-- Table 8: LaptopCategories (Junction table - N-N relationship between Laptops and Categories)
CREATE TABLE LaptopCategories (
    LaptopID INT FOREIGN KEY REFERENCES Laptops(LaptopID),
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID),
    PRIMARY KEY (LaptopID, CategoryID)
);

-- Table 9: LaptopSuppliers (Junction table - N-N relationship between Laptops and Suppliers)
CREATE TABLE LaptopSuppliers (
    LaptopID INT FOREIGN KEY REFERENCES Laptops(LaptopID),
    SupplierID INT FOREIGN KEY REFERENCES Suppliers(SupplierID),
    SupplyDate DATETIME DEFAULT GETDATE(),
    SupplyPrice DECIMAL(18,2),
    PRIMARY KEY (LaptopID, SupplierID)
);

-- Insert sample data

-- Users
INSERT INTO Users (Username, Password, Role, FullName, Email) VALUES
('admin', 'admin123', 'Admin', 'Administrator', 'admin@laptopshop.com'),
('sales1', 'sales123', 'Sales', 'John Sales', 'john@laptopshop.com'),
('manager1', 'manager123', 'Manager', 'Jane Manager', 'jane@laptopshop.com'),
('warehouse1', 'warehouse123', 'Warehouse', 'Bob Warehouse', 'bob@laptopshop.com');

-- Customers
INSERT INTO Customers (CustomerName, Email, Phone, Address) VALUES
('Nguyen Van A', 'nguyenvana@email.com', '0901234567', '123 Le Loi, HCMC'),
('Tran Thi B', 'tranthib@email.com', '0912345678', '456 Nguyen Hue, HCMC'),
('Le Van C', 'levanc@email.com', '0923456789', '789 Tran Hung Dao, Hanoi');

-- Categories
INSERT INTO Categories (CategoryName, Description) VALUES
('Gaming', 'High-performance laptops for gaming'),
('Business', 'Professional laptops for business use'),
('Ultrabook', 'Thin and light laptops'),
('Workstation', 'High-end laptops for professional work');

-- Suppliers
INSERT INTO Suppliers (SupplierName, ContactPerson, Email, Phone, Address) VALUES
('Dell Vietnam', 'Nguyen Van D', 'dell@supplier.com', '0281234567', 'District 1, HCMC'),
('HP Vietnam', 'Tran Van E', 'hp@supplier.com', '0282345678', 'District 3, HCMC'),
('Lenovo Vietnam', 'Le Thi F', 'lenovo@supplier.com', '0283456789', 'District 5, HCMC');

-- Laptops
INSERT INTO Laptops (Brand, Model, Processor, RAM, Storage, GPU, Price, Stock, Description) VALUES
('Dell', 'XPS 15', 'Intel Core i7-13700H', '16GB DDR5', '512GB SSD', 'NVIDIA RTX 4050', 35000000, 10, 'Premium laptop for professionals'),
('HP', 'Omen 16', 'Intel Core i7-13700HX', '16GB DDR5', '1TB SSD', 'NVIDIA RTX 4060', 38000000, 8, 'High-performance gaming laptop'),
('Lenovo', 'ThinkPad X1 Carbon', 'Intel Core i7-1365U', '16GB LPDDR5', '512GB SSD', 'Intel Iris Xe', 32000000, 15, 'Ultra-portable business laptop'),
('Dell', 'G15', 'AMD Ryzen 7 7840HS', '16GB DDR5', '512GB SSD', 'NVIDIA RTX 4050', 28000000, 12, 'Affordable gaming laptop'),
('HP', 'Pavilion 15', 'Intel Core i5-1335U', '8GB DDR4', '512GB SSD', 'Intel Iris Xe', 18000000, 20, 'Budget-friendly laptop');

-- Orders
INSERT INTO Orders (CustomerID, TotalAmount, Status, Notes) VALUES
(1, 35000000, 'Completed', 'First purchase'),
(2, 76000000, 'Processing', 'Buying two laptops'),
(3, 32000000, 'Pending', 'Waiting for stock');

-- OrderDetails
INSERT INTO OrderDetails (OrderID, LaptopID, Quantity, UnitPrice) VALUES
(1, 1, 1, 35000000),
(2, 2, 2, 38000000),
(3, 3, 1, 32000000);

-- LaptopCategories (N-N relationships)
INSERT INTO LaptopCategories (LaptopID, CategoryID) VALUES
(1, 2), (1, 3), -- Dell XPS 15: Business, Ultrabook
(2, 1), -- HP Omen 16: Gaming
(3, 2), (3, 3), -- ThinkPad X1: Business, Ultrabook
(4, 1), -- Dell G15: Gaming
(5, 2); -- HP Pavilion: Business

-- LaptopSuppliers (N-N relationships)
INSERT INTO LaptopSuppliers (LaptopID, SupplierID, SupplyPrice) VALUES
(1, 1, 30000000), -- Dell XPS from Dell Vietnam
(2, 2, 33000000), -- HP Omen from HP Vietnam
(3, 3, 28000000), -- ThinkPad from Lenovo Vietnam
(4, 1, 24000000), -- Dell G15 from Dell Vietnam
(5, 2, 15000000); -- HP Pavilion from HP Vietnam

GO
