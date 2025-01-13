using System;
namespace POS.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }  // Price field
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
    }


}

