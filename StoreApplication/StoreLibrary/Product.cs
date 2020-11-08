using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    public class Product: IProduct
    {
        public string UniqueID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        // default constructor
        public Product(string ID, string name, string category, double price, int quantity)
        {
            UniqueID = ID;
            Name = name;
            Category = category;
            Price = price;
            Quantity = quantity;
        }

    }
}
