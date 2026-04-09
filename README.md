# Inventory Management System

A C# Console Application that implements a CLI-Based Inventory Management System using Object-Oriented Programming principles.

## How to Run

1. Open your terminal in Visual Studio.
2. Navigate to the folder containing the `.csproj` file.
3. Run the following command: <br>
"dotnet run"

## Default Login Credentials
Username: admin <br>
Password: admin123

## Features
Category Management: Add and view product categories.

Supplier Management: Add and view suppliers with strict phone number validation (must be 11 digits and start with "09").

Product Management: Complete CRUD operations (Add, View, Search, Update, Delete). Validates that selected categories and suppliers actually exist.

Stock Operations: Restock and deduct stock. Prevents deducting more stock than what is available.

Transaction History: Automatically logs every Add, Update, Delete, Restock, and Deduct action with a timestamp, the stock before/after, and the user who did it.

Reports: View a dedicated list of low-stock items (based on custom thresholds) and calculate the total monetary value of the current inventory.
