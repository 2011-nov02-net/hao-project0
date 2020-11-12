﻿using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApplication.UnitTests
{
    public class OrderTests
    {
        [Fact]
        public void CreateAOrder()
        {
            
            List<IProduct> supply = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 10),
                                                    new Product("222", "orange", "Produce", 0.88, 10) };
            List<IProduct> p = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 4),
                                                    new Product("222", "orange", "Produce", 0.88, 4)};
            Store store = new Store("Phoenix101",supply);
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);         
            Order order = new Order(store, customer, DateTime.Today, p);
            Assert.Equal("Phoenix101", order.StoreLocation.BranchID);
            Assert.Equal("123123121", order.Customer.Social);
            Assert.Equal(DateTime.Today, order.OrderedTime);
            Assert.Equal(p,order.ProductList);
        }
    }
}