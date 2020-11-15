using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApplication.UnitTests
{
    /// <summary>
    /// unit test cases for console product class
    /// </summary>
    public class ProductTests
    {
        /// <summary>
        /// testing its constructor
        /// </summary>
        [Fact]
        public void CreateAProduct()
        {
            CProduct p = new CProduct("111","Banana","Produce",0.5,10);
            Assert.Equal("111",p.UniqueID);
            Assert.Equal("Banana", p.Name);
            Assert.Equal("Produce", p.Category);
            Assert.Equal(0.5, p.Price);
            Assert.Equal(10, p.Quantity);

            
        }
    }
}
