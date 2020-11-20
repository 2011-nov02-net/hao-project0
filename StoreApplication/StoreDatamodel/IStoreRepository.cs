using System;
using System.Collections.Generic;
using System.Text;
using StoreLibrary;

namespace StoreDatamodel
{
    public interface IStoreRepository
    {
        // all get methods pull data from db and return library model objects
        CStore GetAStore(string storeLoc);
        CStore GetAStoreInventory(string storeLoc);         
        List<CCustomer> GetAllCustomersAtOneStore(string storeLoc);
        List<COrder> GetAllOrdersOfOneCustomer(string customerid);
        List<CProduct> GetAllProductsOfOneOrder(string orderid);

        // all add methods take library model objects, convert them to dbcontext objects and map them to db
        void StoreAddOneCustomer(CCustomer newCustomer);

        void CustomerPlaceOneOrder(COrder newOrder);

        // other methods tbd

    }
}
