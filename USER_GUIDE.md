# Laptop Shop Management System - User Guide

## Table of Contents
1. [Getting Started](#getting-started)
2. [Login](#login)
3. [Dashboard](#dashboard)
4. [Laptop Management](#laptop-management)
5. [Order Management](#order-management)
6. [Customer Management](#customer-management)
7. [Role-Based Features](#role-based-features)
8. [Troubleshooting](#troubleshooting)

---

## Getting Started

### System Requirements
- Windows 10/11
- .NET 8.0 Runtime or higher
- SQL Server (LocalDB, Express, or Standard)
- 4GB RAM minimum
- 100MB free disk space

### First Time Setup
1. Install SQL Server if not already installed
2. Run the database creation script:
   - Open SQL Server Management Studio (SSMS)
   - Connect to your SQL Server instance
   - Open `LaptopShopApp/Database/CreateDatabase.sql`
   - Execute the script (F5)
3. Launch `LaptopShopApp.exe`

---

## Login

### Login Screen
When you start the application, you'll see the login screen with:
- Username field
- Password field
- Login button
- Demo account credentials displayed

### Demo Accounts
Use these credentials to test different roles:

| Role      | Username   | Password     | Permissions |
|-----------|------------|--------------|-------------|
| Admin     | admin      | admin123     | Full access to everything |
| Manager   | manager1   | manager123   | Manage laptops, view orders/customers |
| Sales     | sales1     | sales123     | Create orders, view laptops/customers |
| Warehouse | warehouse1 | warehouse123 | Update laptop stock |

### Login Steps
1. Enter your username
2. Enter your password
3. Click the **LOGIN** button
4. If credentials are correct, you'll be redirected to the Dashboard

### Login Errors
- **"Please enter username and password"**: Both fields are required
- **"Invalid username or password"**: Credentials don't match any user
- **"Error: [message]"**: Database connection issue

---

## Dashboard

### Overview
The Dashboard is your home screen after login. It displays:
- Welcome message with your name and role
- Three statistics cards:
  - **Total Laptops**: Number of laptops in inventory
  - **Total Orders**: Number of orders in the system
  - **Total Customers**: Number of registered customers
- Navigation menu on the left
- Logout button in the top-right corner

### Navigation Menu
The menu shows available options based on your role:
- **📱 Laptop Management**: View and manage laptop inventory
- **🛒 Order Management**: View and manage orders
- **👥 Customer Management**: View and manage customers

Disabled menu items appear grayed out if you don't have permission.

### Logout
Click the **Logout** button to:
1. End your current session
2. Return to the login screen
3. Require login to access the system again

---

## Laptop Management

### Accessing Laptop Management
- From Dashboard, click **📱 Laptop Management** in the left menu
- A new window will open

### Window Layout
The Laptop Management window has three sections:

#### 1. Action Buttons (Top)
- **➕ Add Laptop**: Start creating a new laptop entry
- **✏️ Edit**: Load selected laptop data into the form
- **🗑️ Delete**: Remove selected laptop from database
- **🔄 Refresh**: Reload laptop list from database

#### 2. Laptop Grid (Middle)
Displays all laptops with columns:
- ID
- Brand
- Model
- Processor
- RAM
- Storage
- GPU
- Price (in VND)
- Stock

**Features**:
- Click column headers to sort
- Click a row to select a laptop
- Alternating row colors for readability

#### 3. Input Form (Bottom)
Form fields for laptop details:
- **Brand**: Manufacturer name (e.g., Dell, HP, Lenovo)
- **Model**: Product model (e.g., XPS 15, ThinkPad X1)
- **Processor**: CPU specification (e.g., Intel Core i7-13700H)
- **RAM**: Memory specification (e.g., 16GB DDR5)
- **Storage**: Storage specification (e.g., 512GB SSD)
- **GPU**: Graphics card (e.g., NVIDIA RTX 4050)
- **Price**: Selling price in VND (numbers only)
- **Stock**: Available quantity (numbers only)
- **Description**: Additional details

### How to Add a Laptop

1. Click **➕ Add Laptop**
2. Fill in all required fields (Brand, Model, Price, Stock)
3. Fill in optional fields (Processor, RAM, Storage, GPU, Description)
4. Click **💾 Save**
5. Confirmation message: "Laptop saved successfully!"
6. New laptop appears in the grid

**Example**:
```
Brand: Asus
Model: ROG Strix G16
Processor: Intel Core i7-13650HX
RAM: 16GB DDR5
Storage: 1TB SSD
GPU: NVIDIA RTX 4060
Price: 35000000
Stock: 5
Description: High-performance gaming laptop
```

### How to Edit a Laptop

1. Select a laptop from the grid (click on the row)
2. Click **✏️ Edit**
3. Laptop data loads into the form
4. Modify the fields you want to change
5. Click **💾 Save**
6. Confirmation message: "Laptop saved successfully!"

### How to Delete a Laptop

1. Select a laptop from the grid
2. Click **🗑️ Delete**
3. Confirmation dialog: "Are you sure you want to delete [Brand Model]?"
4. Click **Yes** to confirm or **No** to cancel
5. If confirmed: "Laptop deleted successfully!"

### Form Buttons

- **💾 Save**: Save the current form data (Add or Update)
- **❌ Clear**: Clear all form fields and deselect

### Validation Rules

- Brand and Model are **required**
- Price must be a valid number
- Stock must be a valid integer
- Price and Stock cannot be negative

---

## Order Management

### Accessing Order Management
- From Dashboard, click **🛒 Order Management** in the left menu

### Window Layout

#### 1. Action Buttons (Top)
- **➕ New Order**: Start creating a new order
- **📄 View Details**: See detailed information about selected order
- **🔄 Refresh**: Reload order list

#### 2. Orders Grid (Middle)
Displays all orders with columns:
- Order ID
- Customer ID
- Order Date
- Total Amount (in VND)
- Status
- Notes

#### 3. Input Form (Bottom)
Form fields for order creation:
- **Customer ID**: ID of the customer placing the order
- **Total Amount**: Total order value in VND
- **Status**: Order status (Pending/Processing/Completed/Cancelled)
- **Notes**: Additional order notes

### How to Create an Order

1. Click **➕ New Order**
2. Enter **Customer ID** (must exist in Customers table)
3. Enter **Total Amount** (in VND)
4. Select **Status** from dropdown (default: Pending)
5. Add any **Notes** (optional)
6. Click **💾 Save**
7. Confirmation: "Order created successfully!"

**Note**: Currently, orders are created at the header level. In a full implementation, you would add order items through OrderDetails.

### How to View Order Details

1. Select an order from the grid
2. Click **📄 View Details**
3. A popup shows:
   - Order ID
   - Customer ID
   - Order Date
   - Status
   - Total Amount
   - List of order items (laptop IDs, quantities, prices)

### Order Status

- **Pending**: Order placed, awaiting processing
- **Processing**: Order being prepared
- **Completed**: Order fulfilled
- **Cancelled**: Order cancelled

### Role Permissions

- **Admin**: Full access
- **Sales**: Can create orders and view details
- **Manager**: Can view orders only
- **Warehouse**: No access

---

## Customer Management

### Accessing Customer Management
- From Dashboard, click **👥 Customer Management** in the left menu

### Window Layout

#### 1. Action Buttons (Top)
- **➕ Add Customer**: Start adding a new customer
- **✏️ Edit**: Load selected customer data
- **🗑️ Delete**: Remove selected customer
- **🔄 Refresh**: Reload customer list

#### 2. Customers Grid (Middle)
Displays all customers with columns:
- ID
- Customer Name
- Email
- Phone
- Address

#### 3. Input Form (Bottom)
Form fields:
- **Customer Name**: Full name (required)
- **Email**: Email address
- **Phone**: Phone number
- **Address**: Physical address

### How to Add a Customer

1. Click **➕ Add Customer**
2. Enter **Customer Name** (required)
3. Enter **Email** (optional)
4. Enter **Phone** (optional)
5. Enter **Address** (optional)
6. Click **💾 Save**
7. Confirmation: "Customer saved successfully!"

**Example**:
```
Customer Name: Tran Van Nam
Email: tranvannam@email.com
Phone: 0909123456
Address: 123 Nguyen Trai, Hanoi
```

### How to Edit a Customer

1. Select a customer from the grid
2. Click **✏️ Edit**
3. Customer data loads into the form
4. Modify the fields
5. Click **💾 Save**
6. Confirmation: "Customer saved successfully!"

### How to Delete a Customer

1. Select a customer from the grid
2. Click **🗑️ Delete**
3. Confirmation dialog: "Are you sure you want to delete customer [Name]?"
4. Click **Yes** to confirm
5. If confirmed: "Customer deleted successfully!"

**Warning**: Deleting a customer with existing orders may cause issues. In production, use soft delete or prevent deletion of customers with orders.

---

## Role-Based Features

### Admin Role
**Full System Access**

✅ Can do:
- Add, edit, delete laptops
- Add, edit, delete customers
- Create and view orders
- Manage all data
- Access all windows

### Manager Role
**Inventory Management Focus**

✅ Can do:
- Add, edit, delete laptops
- View customers (read-only)
- View orders (read-only)

❌ Cannot:
- Create or modify orders
- Delete customers

### Sales Role
**Sales Operations Focus**

✅ Can do:
- Create new orders
- View orders
- View laptops (read-only)
- View customers (read-only)

❌ Cannot:
- Modify laptop inventory
- Delete anything
- Modify customers

### Warehouse Role
**Stock Management Focus**

✅ Can do:
- Update laptop stock
- View laptops

❌ Cannot:
- Access orders
- Access customers
- Add or delete laptops (only update stock)

### Permission Denied Messages
If you try an action without permission, you'll see:
- "You don't have permission to [action] [entity]"
- Example: "You don't have permission to delete laptops"

---

## Troubleshooting

### Cannot Login
**Problem**: "Invalid username or password"

**Solutions**:
1. Check username spelling
2. Check password (case-sensitive)
3. Try demo accounts listed on login screen
4. Verify database is running

### Database Connection Error
**Problem**: "Error: [database-related message]"

**Solutions**:
1. Verify SQL Server is running
2. Check connection string in `DatabaseConnection.cs`
3. Run database creation script again
4. Check SQL Server authentication settings

### Empty Data Grids
**Problem**: No data appears in grids

**Solutions**:
1. Click **🔄 Refresh** button
2. Check if database has data
3. Run sample data inserts from CreateDatabase.sql
4. Verify database connection

### Cannot Save Data
**Problem**: "Failed to save [entity]"

**Solutions**:
1. Check all required fields are filled
2. Verify number fields have valid numbers
3. Check database connection
4. Review validation error messages

### Permission Issues
**Problem**: Buttons are disabled or show permission denied

**Solutions**:
1. Verify your current role
2. Check role permissions in this guide
3. Login with appropriate account for the task
4. Contact administrator if permissions are incorrect

### Window Won't Open
**Problem**: Clicking menu doesn't open window

**Solutions**:
1. Check if window is already open (minimize/restore)
2. Verify you have permission for that window
3. Try logging out and back in
4. Restart the application

### Data Not Refreshing
**Problem**: Changes don't appear immediately

**Solutions**:
1. Click **🔄 Refresh** button in relevant window
2. Close and reopen the window
3. Return to Dashboard and reopen the window

---

## Tips and Best Practices

### General Tips
- Always use **🔄 Refresh** after making changes in other windows
- Use **❌ Clear** button to reset forms
- Double-check data before clicking **💾 Save**
- Use appropriate status values for orders
- Keep customer information up to date

### Data Entry Tips
- Use consistent naming for brands (e.g., always "Dell" not "dell" or "DELL")
- Format prices without commas (enter 35000000 not 35,000,000)
- Use descriptive model names
- Add detailed specifications for better searchability
- Add notes to orders for reference

### Security Tips
- Logout when finished
- Don't share account credentials
- Use appropriate accounts for different tasks
- Don't delete data unless absolutely necessary

---

## Keyboard Shortcuts

| Action | Shortcut |
|--------|----------|
| Navigate between fields | Tab |
| Submit form | Enter (when in input field) |
| Cancel/Clear | Esc |
| Close window | Alt+F4 |

---

## Support

For additional help:
1. Review this User Guide
2. Check DESIGN_PATTERNS.md for technical details
3. Review DATABASE_SCHEMA.md for data structure
4. Contact system administrator

---

## Appendix: Common Scenarios

### Scenario 1: New Laptop Arrival
1. Login as **Manager** or **Admin**
2. Go to Laptop Management
3. Click Add Laptop
4. Enter all laptop details
5. Set initial stock quantity
6. Save

### Scenario 2: Customer Places Order
1. Login as **Sales** or **Admin**
2. Go to Customer Management (if new customer)
3. Add customer if needed
4. Note the Customer ID
5. Go to Order Management
6. Create new order with customer ID
7. Save order

### Scenario 3: Stock Update
1. Login as **Warehouse** or **Manager** or **Admin**
2. Go to Laptop Management
3. Select laptop to update
4. Edit the laptop
5. Change Stock value
6. Save

### Scenario 4: Order Status Update
1. Login as **Admin**
2. Go to Order Management
3. Select order
4. View current status
5. Note: Status updates require database update or additional functionality

---

*Last Updated: 2024*
*Version: 1.0*
