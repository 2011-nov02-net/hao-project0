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
     
        [Fact]
        public void SearchCustomerByNameShouldReturnProfile()
        {
            // arrange
            List<IProduct> supply = new List<IProduct> 
            { new Product("111","Banana","Produce",0.5,10),
              new Product("222","orange","Produce",0.88,10)};
            List<IProduct> p = new List<IProduct> { new Product("111", "Banana", "Produce", 0.5, 4),
                                                    new Product("222", "orange", "Produce", 0.88, 4)};
            Store store= new Store("Phoenix101",supply);
            Customer customer = new Customer("123123121","John","Smith","6021111111",store);

            Order order = new Order(store,customer,DateTime.Today,p);
            customer.PlaceOrder(store,order);
            ISearch searchTool = new NameSearch();
            // act
            bool result = searchTool.Search(store, customer);


            // assert
            Assert.True(result);
          
        }

    }
}
