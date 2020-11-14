using System;
using System.Collections.Generic;

#nullable disable

namespace StoreDatamodel
{
    public partial class Product
    {
        public Product()
        {
            Inventories = new HashSet<Inventory>();
            Orderproducts = new HashSet<Orderproduct>();
        }

        public string Productid { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<Orderproduct> Orderproducts { get; set; }
    }
}
