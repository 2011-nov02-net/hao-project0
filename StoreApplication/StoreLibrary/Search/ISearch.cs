using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary.Search
{
    /// <summary>
    /// search interface to map out several search methods
    /// </summary>
    public interface ISearch
    {
        // string info or Customer customer
        bool Search(CStore storeLocation, CCustomer customer);

    }
}
