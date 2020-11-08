using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary.Search
{
    public class NameSearch : ISearch
    {
        public bool Search(Store storeLocation, Customer customer)
        {
            foreach (var pair in storeLocation.CustomerDict)
            {
                if (customer.Social == pair.Key)
                {
                    return true;
                }               
            }
            return false;
        }
    }
}
