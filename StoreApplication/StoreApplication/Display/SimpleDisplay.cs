using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApplication.Display
{
    public class SimpleDisplay : IDisplay
    {

        public void DisplayOneOrder(Order order)
        {
            string location = order.StoreLocation.BranchID;
            string name = order.Customer.FirstName + " " + order.Customer.LastName;
            DateTime orderedTime = order.OrderedTime;
            string productDetail = "ProductID\tProduct Name\tPice\tQuantity\n";
            foreach (var product in order.ProductList)
            {
                productDetail = productDetail +   product.UniqueID + "\t\t" + product.Name + "\t\t" + product.Price + "\t" + product.Quantity + "\n";
            }
            Console.WriteLine($"Order detail: from {location} customer name:{name} at time:{orderedTime}:\n{productDetail}  ");
        }

        public void DisplayAllOrder(List<Order> orders)
        {
            foreach (var order in orders)
            {
                DisplayOneOrder(order);
            }
           
        }

        
    }
}
