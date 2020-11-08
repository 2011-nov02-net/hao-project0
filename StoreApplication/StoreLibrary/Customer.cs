using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace StoreLibrary
{
    public class Customer
    {
        // unique for a dictionary
        public string Social { get; set; }
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public Store DefaultLocation { get; set; }

        // keep track of a customer's orders
        // change to Ienumerable ??
        public List<Order> OrderHistory { get; set; }

        // does not initialize orderHistory
        public Customer(string social,string firstName, string lastName, string phoneNumber, Store defaultLocation)
        {
            Social = social;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            DefaultLocation = defaultLocation;
            OrderHistory = new List<Order>();
        }

        // help method for testing
        public void UpdateOrderHistory(Order order)
        {
            OrderHistory.Add(order);
        }

        // customer can place order to a store location 
        // implemented here?
        public void PlaceOrder(Store storeLocation, Order newOrder )     
        {
            storeLocation.UpdateStoreOrder(newOrder);
            UpdateOrderHistory(newOrder);
        }
        

    }
}
