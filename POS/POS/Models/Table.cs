using System;
namespace POS.Models
{
    public class Table
    {
        public int TableId { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

}

