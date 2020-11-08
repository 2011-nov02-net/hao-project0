using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    public class Order
    {

        public Store StoreLocation { get; set; }

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
                if (quantity >= 200)
                {
                    // how to reject
                    // try set productList and catch exception to handle rejection
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

        // methods
        // rejects orders with unreasonably high product quantities

        // additional helper functions to add and remove some products to the productList
        public void AddOneProduct(IProduct product)
        {
            ProductList.Add(product);
            // productList.Add(product);
        }

        // remove by type ? by a unique ID ?
        public void RemoveOneProduct(IProduct product)
        { 
            
        }

    }
}
