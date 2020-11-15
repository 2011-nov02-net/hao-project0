using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary.Search
{
    public class NameSearch : ISearch
    {
        /// <summary>
        /// default search by name
        /// </summary>
        /// <param name="storeLocation"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool Search(CStore storeLocation, CCustomer customer)
        {
            foreach (var pair in storeLocation.CustomerDict)
            {
                CCustomer cust= pair.Value;
                if (customer.FirstName == cust.FirstName && customer.LastName == cust.LastName )
                {
                    return true;
                }               
            }
            return false;
        }
    }
}
