﻿using StoreApplication.Display;
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
            Store store = new Store("Phoenix101", supply);
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            // orders the same as the store's inventory
            Order order = new Order(store, customer, DateTime.Today, supply);
            SimpleDisplay dis = new SimpleDisplay();
           
            string path = "../../../SimplyWriteData.json";
            JsonFilePersist persist = new JsonFilePersist(path);
            customer.PlaceOrder(store, order);
            persist.WriteStoreData(store);
            dis.DisplayAllOrder(store.CustomerDict["123123121"].OrderHistory);
        }
        
        [Fact]
        public void SimplyReadData()
        {
            string path = "../../../SimplyWriteData.json";
            JsonFilePersist persist = new JsonFilePersist(path);
            Store store = persist.ReadStoreData();
            foreach (var product in store.CustomerDict["123123121"].OrderHistory[0].ProductList)
            {
                Assert.Equal(0, product.Quantity);
            }
        }
        

        

    }
}
