using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace StoreLibrary
{
    /// <summary>
    /// console customer class, has one behavior to place an order
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
        /// property to reference a default location where a customer makes purchases
        /// </summary>
        [JsonIgnore]
        public CStore DefaultLocation { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public CCustomer()
        { }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        public CCustomer(string customerid,string firstName, string lastName, string phoneNumber, CStore defaultLocation)
        {
            Customerid = customerid;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            DefaultLocation = defaultLocation;        
        }

        /// <summary>
        /// customer's behavior to place an order at a store   
        /// <summary>
        public void PlaceOrder(CStore storeLocation, COrder newOrder )     
        {
            storeLocation.UpdateCustomerOrder(newOrder);
        }
        

    }
}
