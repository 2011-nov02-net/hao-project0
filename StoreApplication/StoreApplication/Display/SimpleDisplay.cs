using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApplication.Display
{
    /// <summary>
    /// default display function
    /// </summary>
    public class SimpleDisplay : IDisplay
    {
        /// <summary>
        /// only display detail of an order
        /// </summary>
        /// <param name="order"></param>
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
        /// <param name="orders"></param>
        public void DisplayAllOrder(List<COrder> orders)
        {
            foreach (var order in orders)
            {
                DisplayOneOrder(order);
            }
           
        }

        
    }
}
