using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApplication.UnitTests
{
    /// <summary>
    /// unit test cases for store class
    /// </summary>
    public class StoreTests
    {
        /// <summary>
        /// testing its constructor
        /// </summary>
        [Fact]
        public void CreateAStore()
        {
            List<CProduct> supply = new List<CProduct>
            { new CProduct("111","Banana","Produce",0.5,10), new CProduct("222","orange","Produce",0.88,10)
            };
            CStore store = new CStore("Phoenix101", "606", supply);

            Assert.Equal("Phoenix101", store.Storeloc );
            foreach (var product in supply)
            {
                Assert.Equal(product.Quantity, store.Inventory[product.UniqueID].Quantity);
            }

        }

        /// <summary>
        /// testing a store's ability to add a customer
        /// </summary>
        [Fact]
        public void StoreAddACustomer()
        {
            List<CProduct> supply = new List<CProduct>
            { new CProduct("111","Banana","Produce",0.5,10),
              new CProduct("222","orange","Produce",0.88,10)};

            CStore store = new CStore("Phoenix101", "606", supply);
            CCustomer customer = new CCustomer("123123121", "John", "Smith", "6021111111");
            store.AddCustomer(customer);
            foreach (var pair in store.CustomerDict)
            {
                if (pair.Key == customer.Customerid)
                    Assert.True(true);
            }
        }

        // updatecustomerorder and checkupdateinventory have been tested in customer tests
    }
}
