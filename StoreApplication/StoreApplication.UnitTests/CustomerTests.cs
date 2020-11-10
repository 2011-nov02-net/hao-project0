using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApplication.UnitTests
{
    public class CustomerTests
    {

        [Fact]
        public void CreateACustomer()
        {
            Store store = new Store("Phoenix101");
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            Assert.Equal("123123121", customer.Social);
            Assert.Equal("John", customer.FirstName);
            Assert.Equal("Smith", customer.LastName);
            Assert.Equal("6021111111", customer.PhoneNumber);
            Assert.Equal(store, customer.DefaultLocation);
        }

        [Fact]
        public void CustomerUpdateOrderHistory()
        {
            Store store = new Store("Phoenix101");
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            List<IProduct> p = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 10),
                                                    new Product("222", "orange", "Produce", 0.88, 10)};        
            Order order = new Order(store, customer, DateTime.Today, p);                  
            customer.UpdateOrderHistory(order);
            for (int i = 0; i < p.Count; i ++)
            {
                Assert.Equal(p[i].UniqueID, customer.OrderHistory[0].ProductList[i].UniqueID);
            }
         
        }

        [Fact]
        public void CustomerPlaceOrder()
        {
            List<IProduct> p = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 10),
                                                    new Product("222", "orange", "Produce", 0.88, 10)};
            Store store = new Store("Phoenix101",p);
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            // orders the same as the store's inventory
            Order order = new Order(store, customer, DateTime.Today, p);

            customer.PlaceOrder(store, order);
            // comparing customer' orderHistroy with store's customerDict
            // making sure the order has been handed from the customer to the store
            for (int i = 0; i < p.Count; i++)
            {
                Assert.Equal(p[i].UniqueID, store.CustomerDict["123123121"].OrderHistory[0].ProductList[i].UniqueID);
            }
        }
    }
}
