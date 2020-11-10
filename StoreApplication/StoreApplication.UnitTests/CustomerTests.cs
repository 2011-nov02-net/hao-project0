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
            // temp
            List<Order> storeOrder = new List<Order>();
            storeOrder.Add(order);
            customer.UpdateOrderHistory(order);
            Assert.Equal(storeOrder,customer.OrderHistory);
        }

        [Fact]
        public void CustomerPlaceOrder()
        {
            List<IProduct> supply = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 10),
                                                    new Product("222", "orange", "Produce", 0.88, 10)};
            Store store = new Store("Phoenix101",supply);
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            // orders the same as the store's inventory
            Order order = new Order(store, customer, DateTime.Today, supply);

            customer.PlaceOrder(store, order);
            // comparing customer' orderHistroy with store's customerDict
            // making sure the order has been handed from the customer to the store
            Assert.Equal(customer.OrderHistory[0].StoreLocation.BranchID, store.CustomerDict["123123121"][0].StoreLocation.BranchID );
            Assert.Equal(customer.OrderHistory[0].Customer.Social,store.CustomerDict["123123121"][0].Customer.Social);
            Assert.Equal(customer.OrderHistory[0].OrderedTime, store.CustomerDict["123123121"][0].OrderedTime);
            Assert.Equal(customer.OrderHistory[0].ProductList, store.CustomerDict["123123121"][0].ProductList);
            
     
        }
    }
}
