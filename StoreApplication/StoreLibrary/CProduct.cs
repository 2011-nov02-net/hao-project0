using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    /// <summary>
    /// product class, has no behaviors
    /// </summary>
    public class CProduct
    {
        /// <summary>
        /// property productid to uniquely identify a product
        /// </summary>
        public string UniqueID { get; set; }

        /// <summary>
        /// property name of a product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// property category of a product
        /// </summary>
        public string Category { get; set; }

        private double price;

        /// <summary>
        /// property price of a product, must set it positive
        /// </summary>
        public double Price
        {
            get { return price; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("price must be positive");
                }
                price = value;
            }
        }

        private int quantity;

        /// <summary>
        /// property quantity of a product, must set it positive
        /// </summary>
        public int Quantity
        {
            get { return quantity; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("quantity must be positive");
                }
                quantity = value;
                
            }
        }
      
        /// <summary>
        /// parameterized constructor for a product
        /// </summary>    
        public CProduct(string ID, string name, string category, double price, int quantity)
        {
            UniqueID = ID;
            Name = name;
            Category = category;
            Price = price;
            Quantity = quantity;
        }

        public CProduct(string ID, string name, string category, double price)
        {
            UniqueID = ID;
            Name = name;
            Category = category;
            Price = price;          
        }
    }
}
