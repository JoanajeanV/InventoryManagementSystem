namespace InventorySystem
{
    class Category
    {
        static int next = 1;
        public int    Id   { get; private set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public Category(string n, string d) { Id = next++; Name = n; Desc = d; }
        public override string ToString() => "[" + Id + "] " + Name + " - " + Desc;
    }
}
