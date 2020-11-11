using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;


namespace StoreLibrary
{
    public class Order
    {
        [JsonIgnore]
        public Store StoreLocation { get; set; }

        [JsonIgnore]
        public Customer Customer { get; set; }

        // keep track of ordered time
        public DateTime OrderedTime { get; set; }

        // multiple kinds of product -> interface  
        private List<IProduct> productList = new List<IProduct>();
      
        public List<IProduct> ProductList {
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
                    // nned to handle rejection
                    throw new ArgumentException("This order contains high quantity of products");
                }
                // set a list
                productList = value;
            } }

        public Order(Store storeLocation, Customer customer,DateTime orderedTime,List<IProduct> productList )
        {
            StoreLocation = storeLocation;
            Customer = customer;
            OrderedTime = orderedTime;
            ProductList = productList;
        }

        // remove by unique ID
        public void RemoveOneProduct(IProduct product)
        { 
            
        }

    }
}
