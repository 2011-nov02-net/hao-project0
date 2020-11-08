using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary.Search
{
    public interface ISearch
    {
        // string info or Customer customer
        bool Search(Store storeLocation, Customer customer);

    }
}
