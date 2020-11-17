using StoreLibrary;
using StoreLibrary.Search;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApplication.UnitTests
{
    /// <summary>
    /// unit test cases for simple search class
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
            CStore store= new CStore("Phoenix101", "606", supply);
            CCustomer customer = new CCustomer("123123121","John","Smith","6021111111");

            COrder order = new COrder(store,customer,DateTime.Today, 100, p);
            customer.PlaceOrder(store,order);
            ISearch searchTool = new SimpleSearch();
            // act
            string customerid;
            bool result = searchTool.SearchByName(store, "John", "Smith", out customerid);


            // assert
            Assert.True(result);
          
        }

    }
}
