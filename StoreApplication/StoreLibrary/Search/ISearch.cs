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
        /// method to search by name, but the result is not unique
        /// method to serach by name and phone, the result is unique
        /// </summary>
        bool SearchByName(CStore storeLocation, string firstname, string lastname, out string customerid);
        bool SearchByNameAndPhone(CStore storeLocation, string firstname, string lastname, string phonenumber out string customerid)
    }
}
