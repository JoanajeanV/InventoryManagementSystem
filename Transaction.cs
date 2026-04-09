using System;
namespace InventorySystem
{
    class Transaction
    {
        static int next = 1;
        public int      Id          { get; private set; }
        public string   ProductName { get; set; }
        public string   Type        { get; set; }
        public int      Quantity    { get; set; }
        public int      StockBefore { get; set; }
        public int      StockAfter  { get; set; }
        public string   DoneBy      { get; set; }
        public DateTime Time        { get; private set; }
        public Transaction(string prod, string type, int qty, int before, int after, string by)
        { Id = next++; ProductName = prod; Type = type; Quantity = qty; StockBefore = before; StockAfter = after; DoneBy = by; Time = DateTime.Now; }
    }
}
