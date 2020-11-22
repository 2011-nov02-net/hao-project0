using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApplication.UnitTests
{
    /// <summary>
    /// unit test cases for console customer class
    /// </summary>
    public class CustomerTests
    {
        /// <summary>
        /// testing its constructor
        /// </summary>
        [Fact]
        public void CreateACustomer()
        {
            CStore store = new CStore("Phoenix101");
            CCustomer customer = new CCustomer("123123121", "John", "Smith", "6021111111");
            Assert.Equal("123123121", customer.Customerid);
            Assert.Equal("John", customer.FirstName);
            Assert.Equal("Smith", customer.LastName);
            Assert.Equal("6021111111", customer.PhoneNumber);        
        }    

        /// <summary>
        /// testing the scenario when a customer placed an order successfully
        /// </summary>
        [Fact]
        public void CustomerPlacedASuccessfulOrder()
        {
            List<CProduct> supply = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 10),
                                                    new CProduct("222", "orange", "Produce", 0.88, 10)};
            List<CProduct> p = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 4),
                                                    new CProduct("222", "orange", "Produce", 0.88, 4)};
            CStore store = new CStore("Phoenix101", "606", supply);
            CCustomer customer = new CCustomer("123123121", "John", "Smith", "6021111111");
            COrder order = new COrder(store, customer, DateTime.Today, 100, p);
            customer.PlaceOrder(store, order);
            // inventory should be updated 10-4=6
            foreach (var item in store.Inventory)
            {
                Assert.Equal(6, item.Value.Quantity);
            }
        }

        /// <summary>
        /// testing the scenario when a guest failed to place an order
        /// </summary>
        [Fact]
        public void CustomerWithoutProfileFailedToPlaceAnOrder()
        {
            List<CProduct> supply = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 10),
                                                    new CProduct("222", "orange", "Produce", 0.88, 10)};
            List<CProduct> p = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 20),
                                                    new CProduct("222", "orange", "Produce", 0.88, 20)};
            CStore store = new CStore("Phoenix101", "606", supply);
            CCustomer customer = new CCustomer("123123121", "John", "Smith", "6021111111");
            COrder order = new COrder(store, customer, DateTime.Today, 100, p);
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

        /// <summary>
        /// testing the scenario when a customer failed to place an order
        /// </summary>
        [Fact]
        public void CustomerWithProfileFailedToPlaceAnOrder()
        {
            List<CProduct> supply = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 10),
                                                    new CProduct("222", "orange", "Produce", 0.88, 10)};
            List<CProduct> p = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 20),
                                                    new CProduct("222", "orange", "Produce", 0.88, 20)};
            CStore store = new CStore("Phoenix101", "606", supply);
            CCustomer customer = new CCustomer("123123121", "John", "Smith", "6021111111");
            COrder order = new COrder(store, customer, DateTime.Today, 100, p);
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

        /// <summary>
        /// testing the scenario when a customer purchased too many products than allowed
        /// </summary>
        [Fact]
        public void CustomerPurchasedTooMany()
        {
            List<CProduct> supply = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 10),
                                                    new CProduct("222", "orange", "Produce", 0.88, 10)};
            List<CProduct> p = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 2000),
                                                    new CProduct("222", "orange", "Produce", 0.88, 2000)};
            CStore store = new CStore("Phoenix101", "606", supply);
            CCustomer customer = new CCustomer("123123121", "John", "Smith", "6021111111");
            try
            {
                COrder order = new COrder(store, customer, DateTime.Today, 100, p);
            }
            catch (ArgumentException e)
            {
                Assert.Equal("This order contains high quantity of products", e.ToString());
            }
        }


    }
}
