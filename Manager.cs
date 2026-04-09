using System;
using System.Collections.Generic;
using System.Linq;

namespace InventorySystem
{
    class Manager
    {
        List<Category>    cats  = new List<Category>();
        List<Supplier>    sups  = new List<Supplier>();
        List<Product>     prods = new List<Product>();
        List<User>        users = new List<User>();
        List<Transaction> txns  = new List<Transaction>();

        public User CurrentUser { get; private set; }

        public Manager()
        {
            users.Add(new User("admin", "admin123", "Admin"));
        }

        // AUTH
        public bool Login(string u, string p)
        {
            User found = users.FirstOrDefault(x => x.Username == u);
            if (found != null && found.CheckPassword(p)) { CurrentUser = found; return true; }
            return false;
        }
        public void Logout() { CurrentUser = null; }

        // CATEGORIES
        public void AddCategory(string n, string d)
        {
            if (string.IsNullOrWhiteSpace(n)) throw new Exception("Name cannot be empty.");
            if (cats.Any(c => c.Name == n))   throw new Exception("Category already exists.");
            cats.Add(new Category(n, d));
        }
        public List<Category> GetCategories()      => cats;
        public Category       FindCategory(int id) => cats.FirstOrDefault(c => c.Id == id);

        // SUPPLIERS
        public void AddSupplier(string n, string c, string p)
        {
            if (string.IsNullOrWhiteSpace(n)) throw new Exception("Name cannot be empty.");
            if (sups.Any(s => s.Name == n))   throw new Exception("Supplier already exists.");
            sups.Add(new Supplier(n, c, p));
        }
        public List<Supplier> GetSuppliers()       => sups;
        public Supplier       FindSupplier(int id) => sups.FirstOrDefault(s => s.Id == id);

        // PRODUCTS
        public void AddProduct(string n, string d, double p, int s, int c, int sp, int t)
        {
            if (string.IsNullOrWhiteSpace(n)) throw new Exception("Name cannot be empty.");
            if (FindCategory(c) == null)       throw new Exception("Category ID " + c + " does not exist.");
            if (FindSupplier(sp) == null)      throw new Exception("Supplier ID " + sp + " does not exist.");
            Product prod = new Product(n, d, p, s, c, sp, t);
            prods.Add(prod);
            Log(prod.Name, "ADD", s, 0, s);
        }
        public List<Product> GetProducts()       => prods;
        public Product       FindProduct(int id) => prods.FirstOrDefault(p => p.Id == id);
        public List<Product> Search(string kw)   => prods.Where(p => p.Name.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

        public void UpdateProduct(int id, string n, string d, double p, int c, int sp, int t)
        {
            Product prod = FindProduct(id) ?? throw new Exception("Product not found.");
            if (FindCategory(c) == null)  throw new Exception("Category ID " + c + " does not exist.");
            if (FindSupplier(sp) == null) throw new Exception("Supplier ID " + sp + " does not exist.");
            prod.Name = n; prod.Desc = d; prod.Price = p; prod.CategoryId = c; prod.SupplierId = sp; prod.Threshold = t;
            Log(prod.Name, "UPDATE", 0, prod.Stock, prod.Stock);
        }

        public void DeleteProduct(int id)
        {
            Product p = FindProduct(id) ?? throw new Exception("Product not found.");
            Log(p.Name, "DELETE", 0, p.Stock, 0);
            prods.Remove(p);
        }

        public void Restock(int id, int qty)
        {
            Product p = FindProduct(id) ?? throw new Exception("Product not found.");
            if (qty <= 0) throw new Exception("Quantity must be positive.");
            int before = p.Stock; p.Stock += qty;
            Log(p.Name, "RESTOCK", qty, before, p.Stock);
        }

        public void Deduct(int id, int qty)
        {
            Product p = FindProduct(id) ?? throw new Exception("Product not found.");
            if (qty <= 0)      throw new Exception("Quantity must be positive.");
            if (qty > p.Stock) throw new Exception("Not enough stock. Available: " + p.Stock);
            int before = p.Stock; p.Stock -= qty;
            Log(p.Name, "DEDUCT", qty, before, p.Stock);
        }

        // REPORTS
        public List<Product>     GetLowStock()     => prods.Where(p => p.IsLow).ToList();
        public double            GetTotalValue()   => prods.Sum(p => p.Price * p.Stock);
        public List<Transaction> GetTransactions() => txns;

        void Log(string name, string type, int qty, int before, int after)
        {
            txns.Add(new Transaction(name, type, qty, before, after, CurrentUser?.Username ?? "system"));
        }
    }
}
