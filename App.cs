using System;
using System.Collections.Generic;

namespace InventorySystem
{
    class App
    {
        Manager mgr = new Manager();

        // ── HELPERS ──────────────────────────────────────────
        static void OK(string s)     { Console.ForegroundColor = ConsoleColor.Green;  Console.WriteLine("  [OK]    " + s); Console.ResetColor(); }
        static void Err(string s)    { Console.ForegroundColor = ConsoleColor.Red;    Console.WriteLine("  [ERROR] " + s); Console.ResetColor(); }
        static void Info(string s)   { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("  [INFO]  " + s); Console.ResetColor(); }
        static void Line()           { Console.WriteLine("  " + new string('-', 55)); }
        static void Header(string t)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  +--------------------------------------------------+");
            Console.WriteLine("  |  " + t.PadRight(48) + "|");
            Console.WriteLine("  +--------------------------------------------------+");
            Console.ResetColor();
        }
        static string Ask(string label)    { Console.Write("  " + label + ": "); return Console.ReadLine()?.Trim() ?? ""; }
        static int    AskInt(string label) { int.TryParse(Ask(label), out int v); return v; }
        static double AskDbl(string label) { double.TryParse(Ask(label), out double v); return v; }
        static void   Divider()            { Console.ForegroundColor = ConsoleColor.DarkGray; Console.WriteLine("\n" + new string('=', 60) + "\n"); Console.ResetColor(); }

        // ── PRODUCT TABLE ─────────────────────────────────────
        void ShowProducts(List<Product> list)
        {
            if (list.Count == 0) { Info("No products found."); return; }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n  ID   Name                  Price      Stock  Category        Supplier        Thresh  Status");
            Console.ResetColor();
            Line();
            foreach (Product p in list)
            {
                string cat = mgr.FindCategory(p.CategoryId)?.Name ?? "?";
                string sup = mgr.FindSupplier(p.SupplierId)?.Name ?? "?";
                Console.ForegroundColor = p.IsLow ? ConsoleColor.Red : ConsoleColor.Gray;
                Console.WriteLine("  " + p.Id.ToString().PadRight(5) + p.Name.PadRight(22) +
                    ("P" + p.Price.ToString("N2")).PadLeft(9) + p.Stock.ToString().PadLeft(8) + "  " +
                    cat.PadRight(16) + sup.PadRight(16) + p.Threshold.ToString().PadLeft(6) + "  " +
                    (p.IsLow ? "LOW" : "OK"));
                Console.ResetColor();
            }
            Line();
        }

        // ── RUN ───────────────────────────────────────────────
        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n  +=====================================+");
            Console.WriteLine("  |   INVENTORY MANAGEMENT SYSTEM      |");
            Console.WriteLine("  +=====================================+");
            Console.ResetColor();

            // LOGIN
            Header("LOGIN");
            bool loggedIn = false;
            for (int i = 1; i <= 3; i++)
            {
                if (mgr.Login(Ask("Username"), Ask("Password")))
                { OK("Welcome, " + mgr.CurrentUser.Username + "!"); loggedIn = true; break; }
                Err("Wrong credentials. Attempts left: " + (3 - i));
            }
            if (!loggedIn) { Err("Too many failed attempts. Exiting."); return; }

            // MAIN MENU
            while (true)
            {
                Divider();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("  INVENTORY SYSTEM  |  User: " + mgr.CurrentUser.Username + "  (" + mgr.CurrentUser.Role + ")\n");
                Console.ResetColor();
                Console.WriteLine("  [1]  Categories       [2]  Suppliers");
                Console.WriteLine("  [3]  Add Product      [4]  View Products");
                Console.WriteLine("  [5]  Search           [6]  Update Product");
                Console.WriteLine("  [7]  Delete Product   [8]  Restock");
                Console.WriteLine("  [9]  Deduct Stock     [10] Transactions");
                Console.WriteLine("  [11] Low Stock        [12] Inventory Value");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  [0]  Logout");
                Console.ResetColor();
                Console.WriteLine();

                string choice = Ask("Choice");
                if      (choice == "1")  Categories();
                else if (choice == "2")  Suppliers();
                else if (choice == "3")  AddProduct();
                else if (choice == "4")  { Header("ALL PRODUCTS");   ShowProducts(mgr.GetProducts()); }
                else if (choice == "5")  { Header("SEARCH");         ShowProducts(mgr.Search(Ask("Keyword"))); }
                else if (choice == "6")  UpdateProduct();
                else if (choice == "7")  DeleteProduct();
                else if (choice == "8")  Restock();
                else if (choice == "9")  Deduct();
                else if (choice == "10") Transactions();
                else if (choice == "11") LowStock();
                else if (choice == "12") InventoryValue();
                else if (choice == "0")  { mgr.Logout(); OK("Logged out. Goodbye!"); return; }
                else                     Err("Invalid choice.");
            }
        }

        // ── CATEGORIES ───────────────────────────────────────
        void Categories()
        {
            Header("CATEGORIES");
            Console.WriteLine("  [1] Add   [2] View");
            try
            {
                if (Ask("Choice") == "1")
                {
                    mgr.AddCategory(Ask("Name"), Ask("Description"));
                    OK("Category added.");
                }
                else
                {
                    List<Category> list = mgr.GetCategories();
                    if (list.Count == 0) Info("No categories yet.");
                    else { Line(); foreach (Category c in list) Console.WriteLine("  " + c); Line(); }
                }
            }
            catch (Exception ex) { Err(ex.Message); }
        }

        // ── SUPPLIERS ────────────────────────────────────────
        void Suppliers()
        {
            Header("SUPPLIERS");
            Console.WriteLine("  [1] Add   [2] View");
            try
            {
                if (Ask("Choice") == "1")
                {
                    string name    = Ask("Name");
                    string contact = Ask("Contact");
                    string phone   = Ask("Phone (11 digits, starts with 09)");
                    if (phone.Length != 11 || !phone.StartsWith("09") || !long.TryParse(phone, out _))
                        throw new Exception("Invalid phone. Must be 11 digits and start with 09.");
                    mgr.AddSupplier(name, contact, phone);
                    OK("Supplier added.");
                }
                else
                {
                    List<Supplier> list = mgr.GetSuppliers();
                    if (list.Count == 0) Info("No suppliers yet.");
                    else { Line(); foreach (Supplier s in list) Console.WriteLine("  " + s); Line(); }
                }
            }
            catch (Exception ex) { Err(ex.Message); }
        }

        // ── ADD PRODUCT ──────────────────────────────────────
        void AddProduct()
        {
            Header("ADD PRODUCT");
            try
            {
                List<Category> cats = mgr.GetCategories();
                if (cats.Count == 0) { Info("No categories yet. Add one first."); return; }
                foreach (Category c in cats) Console.WriteLine("  " + c);
                int catId = AskInt("Category ID");
                if (mgr.FindCategory(catId) == null) throw new Exception("Category ID " + catId + " does not exist.");

                List<Supplier> sups = mgr.GetSuppliers();
                if (sups.Count == 0) { Info("No suppliers yet. Add one first."); return; }
                foreach (Supplier s in sups) Console.WriteLine("  " + s);
                int supId = AskInt("Supplier ID");
                if (mgr.FindSupplier(supId) == null) throw new Exception("Supplier ID " + supId + " does not exist.");

                string name  = Ask("Product Name");
                string desc  = Ask("Description");
                double price = AskDbl("Price");
                int    stock = AskInt("Stock");
                int    thresh = AskInt("Low-Stock Threshold (default 5)");
                if (thresh <= 0) thresh = 5;

                mgr.AddProduct(name, desc, price, stock, catId, supId, thresh);
                OK("Product added.");
            }
            catch (Exception ex) { Err(ex.Message); }
        }

        // ── UPDATE PRODUCT ───────────────────────────────────
        void UpdateProduct()
        {
            Header("UPDATE PRODUCT");
            try
            {
                ShowProducts(mgr.GetProducts());
                int id = AskInt("Product ID");
                Product p = mgr.FindProduct(id);
                if (p == null) throw new Exception("Product ID " + id + " does not exist.");

                string n  = Ask("Name ["      + p.Name       + "]");
                string d  = Ask("Desc ["      + p.Desc       + "]");
                string pr = Ask("Price ["     + p.Price      + "]");
                string ci = Ask("Cat ID ["    + p.CategoryId + "]");
                string si = Ask("Sup ID ["    + p.SupplierId + "]");
                string t  = Ask("Threshold [" + p.Threshold  + "]");

                int catId = ci == "" ? p.CategoryId : int.Parse(ci);
                int supId = si == "" ? p.SupplierId : int.Parse(si);
                if (mgr.FindCategory(catId) == null) throw new Exception("Category ID " + catId + " does not exist.");
                if (mgr.FindSupplier(supId) == null) throw new Exception("Supplier ID " + supId + " does not exist.");

                mgr.UpdateProduct(id,
                    n  == "" ? p.Name      : n,
                    d  == "" ? p.Desc      : d,
                    pr == "" ? p.Price     : double.Parse(pr),
                    catId, supId,
                    t  == "" ? p.Threshold : int.Parse(t));
                OK("Product updated.");
            }
            catch (Exception ex) { Err(ex.Message); }
        }

        // ── DELETE PRODUCT ───────────────────────────────────
        void DeleteProduct()
        {
            Header("DELETE PRODUCT");
            try
            {
                ShowProducts(mgr.GetProducts());
                int id = AskInt("Product ID");
                Product p = mgr.FindProduct(id);
                if (p == null) throw new Exception("Product ID " + id + " does not exist.");
                Console.Write("  Delete '" + p.Name + "'? (y/n): ");
                if (Console.ReadLine()?.Trim().ToLower() == "y") { mgr.DeleteProduct(id); OK("Product deleted."); }
                else Info("Cancelled.");
            }
            catch (Exception ex) { Err(ex.Message); }
        }

        // ── RESTOCK ──────────────────────────────────────────
        void Restock()
        {
            Header("RESTOCK");
            try
            {
                ShowProducts(mgr.GetProducts());
                int id = AskInt("Product ID");
                if (mgr.FindProduct(id) == null) throw new Exception("Product ID " + id + " does not exist.");
                mgr.Restock(id, AskInt("Qty to add"));
                OK("Restocked.");
            }
            catch (Exception ex) { Err(ex.Message); }
        }

        // ── DEDUCT ───────────────────────────────────────────
        void Deduct()
        {
            Header("DEDUCT STOCK");
            try
            {
                ShowProducts(mgr.GetProducts());
                int id = AskInt("Product ID");
                if (mgr.FindProduct(id) == null) throw new Exception("Product ID " + id + " does not exist.");
                mgr.Deduct(id, AskInt("Qty to deduct"));
                OK("Stock deducted.");
            }
            catch (Exception ex) { Err(ex.Message); }
        }

        // ── TRANSACTIONS ─────────────────────────────────────
        void Transactions()
        {
            Header("TRANSACTION HISTORY");
            List<Transaction> list = mgr.GetTransactions();
            if (list.Count == 0) { Info("No transactions yet."); return; }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n  ID   Time               Product              Type      Qty   Before   After  By");
            Console.ResetColor();
            Line();
            foreach (Transaction t in list)
            {
                if      (t.Type == "RESTOCK") Console.ForegroundColor = ConsoleColor.Green;
                else if (t.Type == "DEDUCT")  Console.ForegroundColor = ConsoleColor.Red;
                else if (t.Type == "DELETE")  Console.ForegroundColor = ConsoleColor.DarkRed;
                else                          Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("  " + t.Id.ToString().PadRight(5) +
                    t.Time.ToString("MM/dd HH:mm:ss").PadRight(19) +
                    t.ProductName.PadRight(21) + t.Type.PadRight(10) +
                    t.Quantity.ToString().PadLeft(4) +
                    t.StockBefore.ToString().PadLeft(8) +
                    t.StockAfter.ToString().PadLeft(8) + "  " + t.DoneBy);
                Console.ResetColor();
            }
            Line();
        }

        // ── LOW STOCK ────────────────────────────────────────
        void LowStock()
        {
            Header("LOW STOCK ITEMS");
            List<Product> list = mgr.GetLowStock();
            if (list.Count == 0) OK("All products are well-stocked.");
            else { Err(list.Count + " product(s) are low on stock!"); ShowProducts(list); }
        }

        // ── INVENTORY VALUE ──────────────────────────────────
        void InventoryValue()
        {
            Header("INVENTORY VALUE");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n  Total Value: P" + mgr.GetTotalValue().ToString("N2"));
            Console.ResetColor();
            Line();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  ID   Name                    Price      Stock         Value");
            Console.ResetColor();
            Line();
            foreach (Product p in mgr.GetProducts())
                Console.WriteLine("  " + p.Id.ToString().PadRight(5) + p.Name.PadRight(24) +
                    ("P" + p.Price.ToString("N2")).PadLeft(9) + p.Stock.ToString().PadLeft(10) +
                    ("P" + (p.Price * p.Stock).ToString("N2")).PadLeft(14));
            Line();
        }
    }
}
