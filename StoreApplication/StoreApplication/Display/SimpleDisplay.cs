using StoreDatamodel;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApplication.Display
{
    /// <summary>
    /// simple approach of all display functions
    /// </summary>
    public class SimpleDisplay : IDisplay
    {
        /// <summary>
        /// only display detail of an order
        /// </summary>
        public void DisplayOneOrder(COrder order)
        {
            string location = order.StoreLocation.Storeloc;
            string name = order.Customer.FirstName + " " + order.Customer.LastName;
            DateTime orderedTime = order.OrderedTime;
            string productDetail = "ProductID\tProduct Name\tPice\tQuantity\n";
            foreach (var product in order.ProductList)
            {
                productDetail = productDetail +   product.UniqueID + "\t\t" + product.Name + "\t\t" + product.Price + "\t" + product.Quantity + "\n";
            }
            Console.WriteLine($"Order detail: from {location} customer name:{name} at time:{orderedTime}:\n{productDetail}  ");
        }

        /// <summary>
        /// display detail of multiple orders
        /// </summary>
        public void DisplayAllOrders(List<COrder> orders)
        {
            foreach (var order in orders)
            {
                DisplayOneOrder(order);
            }
           
        }

        /// <summary>
        /// diaplay location and phone numbers of all stores
        /// </summary>
        public void DisplayAllStores(List<CStore> stores)
        {
            foreach (var store in stores)
            {
                Console.WriteLine($" Store address: {store.Storeloc}, phone number: {store.Storephone} ");
            }
            Console.WriteLine();
        }



    }
}
