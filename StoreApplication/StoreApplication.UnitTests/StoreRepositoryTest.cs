using Microsoft.EntityFrameworkCore;
using StoreDatamodel;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace StoreApplication.UnitTests
{
    public class StoreRepositoryTest
    {

        static DbContextOptions<Project0databaseContext> option;

        [Fact]
        public void DBAddAStore()
        {
            // setup
            var optionsBuilder = new DbContextOptionsBuilder<Project0databaseContext>();
            optionsBuilder.UseSqlServer(GetConnectionString());
            option = optionsBuilder.Options;

            IStoreRepository repo = new StoreRepository(option);       
            CStore newStore = new CStore("Mountain View 1", "6026626662");

            // action
            repo.AddOneStore(newStore);

            // asert
            using var context = new Project0databaseContext(option);
            var dbStore = context.Stores.First(x => x.Storeloc == "Mountain View 1");
            Assert.Equal(newStore.Storephone, dbStore.Storephone);
            Assert.Empty(dbStore.Storecustomers);
        }

        [Fact]
        public void DBAddOneProduct() {
            var optionsBuilder = new DbContextOptionsBuilder<Project0databaseContext>();
            optionsBuilder.UseSqlServer(GetConnectionString());
            option = optionsBuilder.Options;

            IStoreRepository repo = new StoreRepository(option);
            CProduct newProduct = new CProduct("Product101", "Duck","Meat",10.0);

            // action
            repo.AddOneProduct(newProduct);

            // asert
            using var context = new Project0databaseContext(option);
            var dbProduct = context.Products.Find("Product101");

            Assert.Equal(newProduct.Name, dbProduct.Name);
            Assert.Equal(newProduct.Category, dbProduct.Category);
            Assert.Equal(newProduct.Price, dbProduct.Price);           
        }

        public void DBAddOneCustomer()
        {
            
        
        }


        static string GetConnectionString()
        {
            string path = "../../../../../../project0-connection-string.json";
            string json;
            try
            {
                json = File.ReadAllText(path);

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"required file {path} not found. should just be the connection string in quotes.");
                throw;
            }

            string connectionString = JsonSerializer.Deserialize<string>(json);
            return connectionString;
        }
    }
}
