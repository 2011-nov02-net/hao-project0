using StoreApplication.Display;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApplication.UnitTests
{
    /// <summary>
    /// unit test cases for persistent data
    /// </summary>
    public class DataTests
    {
        /// <summary>
        /// testing the ability to simply write data
        /// </summary>     
        [Fact]
        public void SimplyWriteData()
        {
            List<CProduct> supply = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 10),
                                                    new CProduct("222", "orange", "Produce", 0.88, 10)};
            List<CProduct> p = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 4),
                                                    new CProduct("222", "orange", "Produce", 0.88, 4)};
            CStore store = new CStore("Phoenix101", "606", supply);
            CCustomer customer = new CCustomer("123123121", "John", "Smith", "6021111111");
            // orders the same as the store's inventory
            COrder order = new COrder(store, customer, DateTime.Today, 100, p);
            SimpleDisplay dis = new SimpleDisplay();
           
            string path = "../../../SimplyWriteData.json";
            JsonFilePersist persist = new JsonFilePersist(path);
            customer.PlaceOrder(store, order);
            persist.WriteStoreData(store);
            
        }
        
        /// <summary>
        /// testing the ability to simply read data
        /// </summary>
        [Fact]
        public void SimplyReadData()
        {
            string path = "../../../SimplyWriteData.json";
            JsonFilePersist persist = new JsonFilePersist(path);
            CStore store = persist.ReadStoreData();
            foreach (var product in store.CustomerDict["123123121"].OrderHistory[0].ProductList)
            {
                Assert.Equal(4, product.Quantity);
            }
        }

        /// <summary>
        /// testing the ability to read and rewrite data
        /// </summary>
        [Fact]
        public void ResupplyAndReorderReadAndWrite()
        {
            string path = "../../../SimplyWriteData.json";
            JsonFilePersist persist = new JsonFilePersist(path);
            CStore store = persist.ReadStoreData();

            List<CProduct> supply = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 10),
                                                    new CProduct("222", "orange", "Produce", 0.88, 10),
                                                new CProduct("333","Rocket","Transport",1000000,15)};
                                                    
            store.AddProducts(supply);
            CCustomer customer = new CCustomer("127137147", "Adam", "Savage", "4801111111");
            List<CProduct> p = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 1),
                                                    new CProduct("222", "orange", "Produce", 0.88, 1)};
            COrder order = new COrder(store, customer, DateTime.Today, 100, p);
            customer.PlaceOrder(store, order);

            persist.WriteStoreData(store);
            foreach (var pair in store.Inventory)
            {
                Assert.Equal(15, pair.Value.Quantity);
            }
        }
 
    }
}
