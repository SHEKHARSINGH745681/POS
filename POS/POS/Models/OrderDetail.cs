using System;
namespace POS.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }  // Navigation property for MenuItem
    }

}

