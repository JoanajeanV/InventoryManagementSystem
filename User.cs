namespace InventorySystem
{
    class User
    {
        string _pass;
        public int    Id       { get; private set; }
        public string Username { get; set; }
        public string Role     { get; set; }
        public User(string u, string p, string r = "Staff") { Id = 1; Username = u; _pass = p; Role = r; }
        public bool CheckPassword(string p) => _pass == p;
        public override string ToString() => "[" + Id + "] " + Username + " (" + Role + ")";
    }
}
