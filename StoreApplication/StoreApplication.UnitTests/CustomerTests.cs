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
        public void CustomerPlacedASuccessfulOrder()
        {
            List<IProduct> supply = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 10),
                                                    new Product("222", "orange", "Produce", 0.88, 10)};
            List<IProduct> p = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 4),
                                                    new Product("222", "orange", "Produce", 0.88, 4)};
            Store store = new Store("Phoenix101",supply);
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            Order order = new Order(store, customer, DateTime.Today, p);
            customer.PlaceOrder(store, order);
            // inventory should be updated 10-4=6
            foreach (var item in store.Inventory)
            {
                Assert.Equal(6, item.Value.Quantity);
            }
        }

        [Fact]
        public void CustomerWithoutProfileFailedToPlaceAnOrder()
        {
            List<IProduct> supply = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 10),
                                                    new Product("222", "orange", "Produce", 0.88, 10)};
            List<IProduct> p = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 20),
                                                    new Product("222", "orange", "Produce", 0.88, 20)};
            Store store = new Store("Phoenix101", supply);
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            Order order = new Order(store, customer, DateTime.Today, p);
            customer.PlaceOrder(store, order);

            // inventory should not be updated 10-20<0 => 10
            foreach (var item in store.Inventory)
            {
                Assert.Equal(10, item.Value.Quantity);
            }
            
            // customer does not have an existing profile 
            // a failed order doesn not create a new user profile
            // userDict should be empty
            // .Equal 0 does not check a collection size
            Assert.Empty(store.CustomerDict);
        }

        [Fact]
        public void CustomerWithProfileFailedToPlaceAnOrder()
        {
            List<IProduct> supply = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 10),
                                                    new Product("222", "orange", "Produce", 0.88, 10)};
            List<IProduct> p = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 20),
                                                    new Product("222", "orange", "Produce", 0.88, 20)};
            Store store = new Store("Phoenix101", supply);
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            Order order = new Order(store, customer, DateTime.Today, p);
            // customer has an existing profile
            store.AddCustomer(customer);
            customer.PlaceOrder(store, order);

            // inventory should not be updated 10-20<0 => 10
            foreach (var item in store.Inventory)
            {
                Assert.Equal(10, item.Value.Quantity);
            }

            // userDict should have customer file, but with no order history
            Assert.Empty(store.CustomerDict["123123121"].OrderHistory);
        }

        [Fact]
        public void CustomerPurchasedTooMany()
        {
            List<IProduct> supply = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 10),
                                                    new Product("222", "orange", "Produce", 0.88, 10)};
            List<IProduct> p = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 2000),
                                                    new Product("222", "orange", "Produce", 0.88, 2000)};
            Store store = new Store("Phoenix101", supply);
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            try
            {
                Order order = new Order(store, customer, DateTime.Today, p);
            }
            catch (ArgumentException e)
            {
                Assert.Equal("This order contains high quantity of products", e.ToString());
            }
        }
    }
}
