using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApplication.UnitTests
{
    public class StoreTests
    {
        [Fact]
        public void CreateAStore()
        {
            List<IProduct> supply = new List<IProduct>
            { new Product("111","Banana","Produce",0.5,10), new Product("222","orange","Produce",0.88,10)
            };
            Store store = new Store("Phoenix101",supply);

            Assert.Equal("Phoenix101", store.BranchID );
            foreach (var product in supply)
            {
                Assert.Equal(product.Quantity, store.Inventory[product.UniqueID]);
            }

        }

        [Fact]
        public void StoreAddACustomer()
        {
            List<IProduct> supply = new List<IProduct>
            { new Product("111","Banana","Produce",0.5,10),
              new Product("222","orange","Produce",0.88,10)};

            Store store = new Store("Phoenix101", supply);
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            store.AddCustomer(customer);
            foreach (var pair in store.CustomerDict)
            {
                if (pair.Key == customer.Social)
                    Assert.True(true);
            }
        }

        // updatestoreorder and checkupdateinventory have been tested in customertests
    }
}
