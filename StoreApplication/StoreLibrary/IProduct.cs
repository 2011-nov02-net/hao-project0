using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    // reserved for different kinds of product types
    // like simpleWriter and detailedWriter
    // only has properties
    public interface IProduct
    {
        string UniqueID { get; set; }
        string Name { get; set; }
        string Category { get; set; }
        double Price { get; set; }
        int Quantity { get; set; }
    }
}
