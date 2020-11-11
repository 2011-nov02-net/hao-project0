using Newtonsoft.Json;
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

        // keep track of a customer's orders
        // change to Ienumerable ??
        public List<Order> OrderHistory { get; set; }

        [JsonIgnore]
        public Store DefaultLocation { get; set; }

        public Customer()
        { OrderHistory = new List<Order>(); }

        public Customer(string social,string firstName, string lastName, string phoneNumber, Store defaultLocation)
        {
            Social = social;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            DefaultLocation = defaultLocation;
            OrderHistory = new List<Order>();
        }
       
        // customer can place order to a store location 
        public void PlaceOrder(Store storeLocation, Order newOrder )     
        {
            storeLocation.UpdateCustomerOrder(newOrder);
        }
        

    }
}
