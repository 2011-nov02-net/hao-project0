using StoreLibrary;
using StoreLibrary.Search;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApplication.UnitTests
{
    public class NameSearchTests
    {
        [Theory]
        [InlineData("s")]
        public void SearchCustomerByNameShouldReturnProfile(string something)
        {
            // arrange
            List<IProduct> supply = new List<IProduct> 
            { new Product("111","apple","Produce",1.0,1),
              new Product("222","orange","Produce",0.88,10)};

            Store store= new Store("Phoenix101",supply);
            Customer customer = new Customer("123123121","John","Smith","6021111111",store);

            Order order = new Order(store,customer,DateTime.Today,supply);
            customer.PlaceOrder(store,order);
            ISearch searchTool = new NameSearch();
            // act
            bool result = searchTool.Search(store, customer);


            // assert
            Assert.True(result);
          
        }

    }
}
