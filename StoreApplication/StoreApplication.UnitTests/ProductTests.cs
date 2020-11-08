using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApplication.UnitTests
{
   
    public class ProductTests
    {
        [Fact]
        public void CreateAProduct()
        {
            Product p = new Product("111","Banana","Produce",0.5,10);
            Assert.Equal("111",p.UniqueID);
            Assert.Equal("Banana", p.Name);
            Assert.Equal("Produce", p.Category);
            Assert.Equal(0.5, p.Price);
            Assert.Equal(10, p.Quantity);

            
        }
    }
}
