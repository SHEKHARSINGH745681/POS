using System;
using POS.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace POS.Data
{// Data/PosDbContext.cs
    public class PosDbContext : DbContext
    {
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Table> Tables { get; set; }  // Add Tables DbSet
    }


}

