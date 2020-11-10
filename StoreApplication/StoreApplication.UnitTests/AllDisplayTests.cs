using StoreApplication.Display;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApplication.UnitTests
{
    public class AllDisplayTests
    {
        // move codes here to Main program for testing
        [Fact]
        public void DisplayOneOrderPrintOnConsole()
        {
            IDisplay dis = new SimpleDisplay();

            List<IProduct> supply = new List<IProduct>
            { new Product("111","apple","Produce",1.0,1),
              new Product("222","orange","Produce",0.88,10)};

            Store store = new Store("Phoenix101", supply);
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            Order order = new Order(store, customer, DateTime.Today, supply);

            dis.DisplayOneOrder(order);
            Assert.True(true);
        }

        [Fact]
        public void DisplayAllOrdersPrintOnConsole()
        {
            IDisplay dis = new SimpleDisplay();

            List<IProduct> supply = new List<IProduct>
            { new Product("111","apple","Produce",1.0,1),
              new Product("222","orange","Produce",0.88,10)};

            Store store = new Store("Phoenix101", supply);
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            Order order = new Order(store, customer, DateTime.Today, supply);
            Order order2 = new Order(store, customer, DateTime.Today, supply);
            List<Order> orders = new List<Order>();
            orders.Add(order); orders.Add(order2);

            dis.DisplayAllOrder(orders);
            Assert.True(true);
        }
}
