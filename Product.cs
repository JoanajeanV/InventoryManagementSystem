namespace InventorySystem
{
    class Product
    {
        static int next = 1;
        double _price; int _stock;
        public int    Id         { get; private set; }
        public string Name       { get; set; }
        public string Desc       { get; set; }
        public int    CategoryId { get; set; }
        public int    SupplierId { get; set; }
        public int    Threshold  { get; set; }
        public double Price
        {
            get { return _price; }
            set { if (value < 0) throw new System.Exception("Price cannot be negative."); _price = value; }
        }
        public int Stock
        {
            get { return _stock; }
            set { if (value < 0) throw new System.Exception("Stock cannot be negative."); _stock = value; }
        }
        public bool IsLow => _stock <= Threshold;
        public Product(string n, string d, double p, int s, int c, int sp, int t = 5)
        { Id = next++; Name = n; Desc = d; Price = p; Stock = s; CategoryId = c; SupplierId = sp; Threshold = t; }
    }
}
