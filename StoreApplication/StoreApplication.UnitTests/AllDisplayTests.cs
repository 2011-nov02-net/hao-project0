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
            { new Product("111","Banana","Produce",0.5,10),
              new Product("222","orange","Produce",0.88,10)};
            List<IProduct> p = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 4),
                                                    new Product("222", "orange", "Produce", 0.88, 4)};
            Store store = new Store("Phoenix101", supply);
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            Order order = new Order(store, customer, DateTime.Today, p);
            dis.DisplayOneOrder(order);
            Assert.True(true);
        }

        [Fact]
        public void DisplayAllOrdersPrintOnConsole()
        {
            IDisplay dis = new SimpleDisplay();
            List<IProduct> supply = new List<IProduct>
            { new Product("111","Banana","Produce",0.5,10),
              new Product("222","orange","Produce",0.88,10)};
            List<IProduct> p = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 4),
                                                    new Product("222", "orange", "Produce", 0.88, 4)};
            Store store = new Store("Phoenix101", supply);
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            List<Order> orders = new List<Order>
            {  new Order(store, customer, DateTime.Today, p),
                new Order(store, customer, DateTime.Today, p) };                
            dis.DisplayAllOrder(orders);
            Assert.True(true);
        }
    }
}
