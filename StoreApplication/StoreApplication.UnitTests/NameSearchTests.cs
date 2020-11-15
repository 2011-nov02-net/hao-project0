using StoreLibrary;
using StoreLibrary.Search;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApplication.UnitTests
{
    /// <summary>
    /// unit test cases for name search class
    /// </summary>
    public class NameSearchTests
    {
        /// <summary>
        /// testing a customer already has a profile
        /// </summary>
        [Fact]
        public void SearchCustomerByNameShouldReturnProfile()
        {
            // arrange
            List<CProduct> supply = new List<CProduct> 
            { new CProduct("111","Banana","Produce",0.5,10),
              new CProduct("222","orange","Produce",0.88,10)};
            List<CProduct> p = new List<CProduct> { new CProduct("111", "Banana", "Produce", 0.5, 4),
                                                    new CProduct("222", "orange", "Produce", 0.88, 4)};
            CStore store= new CStore("Phoenix101",supply);
            CCustomer customer = new CCustomer("123123121","John","Smith","6021111111",store);

            COrder order = new COrder(store,customer,DateTime.Today,p);
            customer.PlaceOrder(store,order);
            ISearch searchTool = new NameSearch();
            // act
            bool result = searchTool.Search(store, customer);


            // assert
            Assert.True(result);
          
        }

    }
}
