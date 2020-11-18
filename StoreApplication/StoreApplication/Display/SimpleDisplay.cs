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

        public void DisplayOneOrder(Orderr dbOrders)
        {
            Console.WriteLine($"Orderred at: {dbOrders.Storeloc} customerid: {dbOrders.Customerid} time: {dbOrders.Orderedtime}");
            foreach (var product in dbOrders.Orderproducts)
            {
                Console.WriteLine($"ID: {product.Product.Productid}, Name: {product.Product.Name}, Price: {product.Product.Price}, Quantity: {product.Quantity}");
            }
        }



    }
}
