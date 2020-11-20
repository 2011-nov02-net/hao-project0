using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary.Search
{
    /// <summary>
    /// simple approach of all search methods
    /// </summary>
    public class SimpleSearch : ISearch
    {
        /// <summary>
        /// simple approach to search by name
        /// </summary>
        public bool SearchByName(CStore storeLocation, string firstname, string lastname, out string customerid)
        {
            foreach (var pair in storeLocation.CustomerDict)
            {
                CCustomer customer= pair.Value;
                if (firstname == customer.FirstName && lastname == customer.LastName )
                {
                    customerid = pair.Key;
                    return true;
                }               
            }
            customerid = "";
            return false;
        }

        public bool SearchByNameAndPhone(CStore storeLocation, string firstname, string lastname, string phonenumber, out string customerid)
        {
            foreach (var pair in storeLocation.CustomerDict)
            {
                CCustomer customer = pair.Value;
                if (firstname == customer.FirstName && lastname == customer.LastName && phonenumber == customer.PhoneNumber)
                {
                    customerid = pair.Key;
                    return true;
                }
            }
            customerid = "";
            return false;
        }
    }
}
