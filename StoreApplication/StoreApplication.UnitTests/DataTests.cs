using StoreApplication.Display;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApplication.UnitTests
{
    public class DataTests
    {
        [Fact]
        public void SimplyWriteData()
        {
            List<IProduct> supply = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 10),
                                                    new Product("222", "orange", "Produce", 0.88, 10)};
            List<IProduct> p = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 4),
                                                    new Product("222", "orange", "Produce", 0.88, 4)};
            Store store = new Store("Phoenix101", supply);
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            // orders the same as the store's inventory
            Order order = new Order(store, customer, DateTime.Today, p);
            SimpleDisplay dis = new SimpleDisplay();
           
            string path = "../../../SimplyWriteData.json";
            JsonFilePersist persist = new JsonFilePersist(path);
            customer.PlaceOrder(store, order);
            persist.WriteStoreData(store);
            
        }
        
        [Fact]
        public void SimplyReadData()
        {
            string path = "../../../SimplyWriteData.json";
            JsonFilePersist persist = new JsonFilePersist(path);
            Store store = persist.ReadStoreData();
            foreach (var product in store.CustomerDict["123123121"].OrderHistory[0].ProductList)
            {
                Assert.Equal(4, product.Quantity);
            }
        }

        [Fact]
        public void ResupplyAndReorderReadAndWrite()
        {
            string path = "../../../SimplyWriteData.json";
            JsonFilePersist persist = new JsonFilePersist(path);
            Store store = persist.ReadStoreData();

            List<IProduct> supply = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 10),
                                                    new Product("222", "orange", "Produce", 0.88, 10),
                                                new Product("333","Rocket","Transport",1000000,15)};
                                                    
            store.AddProducts(supply);
            Customer customer = new Customer("127137147", "Adam", "Savage", "4801111111", store);
            List<IProduct> p = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 1),
                                                    new Product("222", "orange", "Produce", 0.88, 1)};
            Order order = new Order(store, customer, DateTime.Today, p);
            customer.PlaceOrder(store, order);

            persist.WriteStoreData(store);
            foreach (var pair in store.Inventory)
            {
                Assert.Equal(15, pair.Value.Quantity);

            }
        }
 
    }
}
