using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary.Search
{
    /// <summary>
    /// search interface to map out several search methods
    /// currently only has search by name
    /// </summary>
    public interface ISearch
    {
        /// <summary>
        /// method to search by name, but name is not unique
        /// need subsequent process to identify a customer
        /// currently it is the first customer found
        /// </summary>
        bool SearchByName(CStore storeLocation, string firstname, string lastname, out string customerid);

    }
}
