namespace InventorySystem
{
    class Supplier
    {
        static int next = 1;
        public int    Id      { get; private set; }
        public string Name    { get; set; }
        public string Contact { get; set; }
        public string Phone   { get; set; }
        public Supplier(string n, string c, string p) { Id = next++; Name = n; Contact = c; Phone = p; }
        public override string ToString() => "[" + Id + "] " + Name + " | " + Contact + " | " + Phone;
    }
}
