using System;
using System.Reflection.Emit;

namespace POS.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
        public int TableId { get; set; }  // Foreign key for Table
        public Table Table { get; set; }  // Navigation property
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }


}

