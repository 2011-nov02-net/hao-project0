using StoreApplication.Display;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApplication.UnitTests
{
    /// <summary>
    /// unit test cases for all display related classes
    /// </summary>
    public class AllDisplayTests
    {
        /// <summary>
        /// testing the ability to simple display an order
        /// </summary>
        // move codes here to Main program for testing
        [Fact]
        public void DisplayOneOrderPrintOnConsole()
        {
            IDisplay dis = new SimpleDisplay();
            List<CProduct> supply = new List<CProduct>
            { new CProduct("111","Banana","Produce",0.5,10),
              new CProduct("222","orange","Produce",0.88,10)};
            List<CProduct> p = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 4),
                                                    new CProduct("222", "orange", "Produce", 0.88, 4)};
            CStore store = new CStore("Phoenix101", "606",supply);
            CCustomer customer = new CCustomer("123123121", "John", "Smith", "6021111111");
            COrder order = new COrder(store, customer, DateTime.Today, 100, p);
            dis.DisplayOneOrder(order);
            Assert.True(true);
        }

        /// <summary>
        /// testing the ability to simple display multiple orders
        /// </summary>
        [Fact]
        public void DisplayAllOrdersPrintOnConsole()
        {
            IDisplay dis = new SimpleDisplay();
            List<CProduct> supply = new List<CProduct>
            { new CProduct("111","Banana","Produce",0.5,10),
              new CProduct("222","orange","Produce",0.88,10)};
            List<CProduct> p = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 4),
                                                    new CProduct("222", "orange", "Produce", 0.88, 4)};
            CStore store = new CStore("Phoenix101", "606",supply);
            CCustomer customer = new CCustomer("123123121", "John", "Smith", "6021111111");
            List<COrder> orders = new List<COrder>
            {  new COrder(store, customer, DateTime.Today, 100, p),
                new COrder(store, customer, DateTime.Today, 100, p) };                
            dis.DisplayAllOrders(orders);
            Assert.True(true);
        }
    }
}
