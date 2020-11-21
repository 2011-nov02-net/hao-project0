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
        List<CProduct> GetInventoryOfAStore(string storeLoc);
        Dictionary<string, CCustomer> GetAllCustomersAtOneStore(string storeLoc);
        List<COrder> GetAllOrdersOfOneCustomer(string customerid, CStore store, CCustomer customer);
        List<CProduct> GetAllProductsOfOneOrder(string orderid);
        CCustomer GetOneCustomerByNameAndPhone(string firstName, string lastName, string phonenumber);
        COrder GetAnOrderByID(string orderid);
        CProduct GetAProductByNameAndCategory(string name, string category);

        // all add methods take library model objects, convert them to dbcontext objects and map them to db
        void StoreAddOneCusomter(string storeLoc, CCustomer customer);

        void CustomerPlaceOneOrder(COrder order, CStore store, double totalCost);
       

        // helper methods to display all store locations, only contain location and store phone number
        List<CStore> GetAllStores();

    }
}
