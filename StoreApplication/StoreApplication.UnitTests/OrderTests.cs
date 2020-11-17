using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApplication.UnitTests
{
    /// <summary>
    /// unit test cases for console order class
    /// </summary>
    public class OrderTests
    {
        /// <summary>
        /// testing its constructor
        /// </summary>
        [Fact]
        public void CreateAOrder()
        {
            
            List<CProduct> supply = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 10),
                                                    new CProduct("222", "orange", "Produce", 0.88, 10) };
            List<CProduct> p = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 4),
                                                    new CProduct("222", "orange", "Produce", 0.88, 4)};
            CStore store = new CStore("Phoenix101", "606", supply);
            CCustomer customer = new CCustomer("123123121", "John", "Smith", "6021111111");         
            COrder order = new COrder(store, customer, DateTime.Today, 100, p);
            Assert.Equal("Phoenix101", order.StoreLocation.Storeloc);
            Assert.Equal("123123121", order.Customer.Customerid);
            Assert.Equal(DateTime.Today, order.OrderedTime);
            Assert.Equal(p,order.ProductList);
        }
    }
}
