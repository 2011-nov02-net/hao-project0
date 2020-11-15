using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;


namespace StoreLibrary
{
    /// <summary>
    /// console order class, has no behaviors
    /// </summary>
    public class COrder
    {
        /// <summary>
        /// property orderid to uniquely identify an order
        /// </summary>
        public string Orderid { get; set; }

        /// <summary>
        /// property to reference a store location
        /// </summary>
        [JsonIgnore]    
        public CStore StoreLocation { get; set; }

        /// <summary>
        /// property to reference a customer
        /// </summary>
        [JsonIgnore]
        public CCustomer Customer { get; set; }

        /// <summary>
        /// property to record date and time of an order
        /// </summary>
        public DateTime OrderedTime { get; set; }

        private double totalCost;

        /// <summary>
        /// property to record total cost of an order, must set it positive
        /// </summary>
        public double TotalCost {
            get { return totalCost; }
            set {
                if (value <= 0)
                {
                    throw new ArgumentException("total cost must be positive");
                }
            } }

        private List<CProduct> productList = new List<CProduct>();

        /// <summary>
        /// property to contain a list of products in an order, total quantity must not exceed 500
        /// </summary>
        public List<CProduct> ProductList {
            get
            { return productList; }
            set
            {
                // rejects orders with unreasonably high product quantities
                int quantity = 0;
                foreach (var p in productList)
                {
                    quantity += p.Quantity;
                }
                if (quantity >= 500)
                {
                    // need to handle rejection
                    throw new ArgumentException("total number of products exceeds maximum");
                }
                // set a list
                productList = value;
            } }

        /// <summary>
        /// default constructor
        /// </summary>
        public COrder()
        { }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        public COrder(CStore storeLocation, CCustomer customer,DateTime orderedTime,List<CProduct> productList )
        {
            StoreLocation = storeLocation;
            Customer = customer;
            OrderedTime = orderedTime;
            ProductList = productList;
        }

        /*
        public void RemoveOneProduct(CProduct product)
        { 
            
        }
        */
    }
}
