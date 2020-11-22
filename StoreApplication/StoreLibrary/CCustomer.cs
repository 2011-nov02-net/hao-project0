using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace StoreLibrary
{
    /// <summary>
    /// customer class, has one behavior to place an order
    /// </summary>
    public class CCustomer
    {
        /// <summary>
        /// property customerid to uniquely identify a customer
        /// </summary>
        public string Customerid { get; set; }

        /// <summary>
        /// property first name of a customer
        /// </summary>
        public string FirstName{ get; set; }

        /// <summary>
        /// property last name of a customer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// property phone number of a customer
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// property to keep track of all orders of a customer
        /// </summary>
        public List<COrder> OrderHistory { get; set; } = new List<COrder>();
    
        /// <summary>
        /// default constructor
        /// </summary>
        public CCustomer()
        { }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        public CCustomer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public CCustomer(string firstName, string lastName, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }

        public CCustomer(string customerid,string firstName, string lastName, string phoneNumber)
        {
            Customerid = customerid;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;    
        }

        /// <summary>
        /// customer's behavior to place an order at a store   
        /// <summary>
        public void PlaceOrder(CStore storeLocation, COrder newOrder )     
        {
            storeLocation.UpdateInventoryAndCustomerOrder(newOrder);
        }
        

    }
}
