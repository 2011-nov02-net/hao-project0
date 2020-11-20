using Microsoft.EntityFrameworkCore;
using StoreLibrary;
using StoreLibrary.IDGenerator;
using StoreLibrary.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoreDatamodel
{
    public class StoreRepository: IStoreRepository
    {
        private readonly DbContextOptions<Project0databaseContext> _contextOptions;
        public StoreRepository(DbContextOptions<Project0databaseContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }

        public IEnumerable<CProduct> GetAllProducts()
        {
            using var context = new Project0databaseContext(_contextOptions);
            IEnumerable<Product> dbProducts = context.Products.ToList();
            IEnumerable<CProduct> conProducts = dbProducts.Select(x => new CProduct(x.Productid, x.Name, x.Category, x.Price, 1));
            return conProducts;
        }

        // M V C design
        // re-implementation seperating business and data-access
        // create a default store with no customer profile and inventory
        public CStore GetAStore(string storeLoc)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.First(x => x.Storeloc == storeLoc);
            CStore store = new CStore(dbStore.Storeloc, dbStore.Storephone);
            return store;
        }

        // create a list of products that can be added to a default store
        public CStore GetAStoreInventory(string storeLoc)
        {
            throw new NotImplementedException();
        }

        public List<CCustomer> GetAllCustomersAtOneStore(string storeLoc)
        {
            throw new NotImplementedException();
        }

        public List<COrder> GetAllOrdersOfOneCustomer(string customerid)
        {
            throw new NotImplementedException();
        }

        public List<CProduct> GetAllProductsOfOneOrder(string orderid)
        {
            throw new NotImplementedException();
        }

        public void StoreAddOneCustomer(CCustomer newCustomer)
        {
            throw new NotImplementedException();
        }

        public void CustomerPlaceOneOrder(COrder newOrder)
        {
            throw new NotImplementedException();
        }


        // incorrect implementation

        public string StoreAddACusomter(string storeLoc, string firstName, string lastName, string phoneNumber)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.Include(x => x.Storecustomers)
                                           .ThenInclude(x=>x.Customer)
                                            .First(x => x.Storeloc == storeLoc);
            // empty store with no customer info
            var conStore = new CStore(dbStore.Storeloc, dbStore.Storephone);
     
            // begin
            foreach (var cust in dbStore.Storecustomers)
            {
                // extra phone number colunmn can be used for search
                CCustomer conCustomer = new CCustomer(cust.Customer.Firstname, cust.Customer.Lastname,cust.Customer.Phonenumber);
                conStore.CustomerDict[cust.Customerid] = conCustomer;
            }
           
            // refactor 
            SimpleSearch ss = new SimpleSearch();
            // "" string if not found    
            string CID = "";
            bool found = ss.SearchByName(conStore, firstName, lastName, out CID);
            if (found)
            {               
            }
            else
            {
                string newCID = CIDGen.Gen();
                CID = newCID;
                var newCustomer = new Customer {
                    Customerid = newCID,
                    Firstname = firstName,
                    Lastname = lastName,
                    Phonenumber = phoneNumber
                };
                context.Customers.Add(newCustomer);       
                context.SaveChanges();

                var newBridge = new Storecustomer
                {
                    Storeloc = storeLoc,
                    Customerid = newCID
                };
                context.Storecustomers.Add(newBridge);
                context.SaveChanges();
            }
            return CID;
        }

        public bool CustomerPlaceAnOrder(string storeLoc, string firstName, string lastName,string phoneNumber,List<CProduct> productList)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.Include(x => x.Inventories)
                                                 .ThenInclude(x => x.Product)
                                                   .First(x => x.Storeloc == storeLoc);
            //.Include(x => x.Storecustomers)
            //.ThenInclude(x => x.Customer)
            //.ThenInclude(x => x.Orderrs)
            //.ThenInclude(x => x.Orderproducts)
            //.ThenInclude(x => x.Product)
            //.ThenInclude(x => x.Inventories)

            var conStore = new CStore(dbStore.Storeloc, dbStore.Storephone);
            List<CProduct> inventory = new List<CProduct>();

            foreach (var product in dbStore.Inventories)
            {
                CProduct p = new CProduct(product.Productid,product.Product.Name, product.Product.Category, product.Product.Price, product.Quantity);               
                inventory.Add(p);
               
            }
            // dictionary "productid":product(contains quantity)
            conStore.AddProducts(inventory);

            // check customer's quantity against inventory quantity
            bool isEnough = conStore.CheckInventory(productList);
            string customerid;
            if (isEnough)
            {
                customerid = StoreAddACusomter(storeLoc, firstName, lastName, phoneNumber);
                // Customer table and Storecustomer table updated

                // update order
                string orderid = OIDGen.Gen();              
                double total = conStore.CalculateTotalPrice(productList);
                var newOrder = new Orderr
                {
                    Orderid = orderid,
                    Storeloc = storeLoc,
                    Customerid = customerid,
                    Orderedtime = DateTime.Now,
                    Totalcost = total
                };
                context.Orderrs.Add(newOrder);
                context.SaveChanges();

                // update Orderproduct  
                foreach (var product in productList)
                {
                    var newOP = new Orderproduct
                    {
                        Orderid = orderid,
                        Productid = product.UniqueID,
                        Quantity = product.Quantity
                    };
                    context.Orderproducts.Add(newOP);
                }
                context.SaveChanges();

                // update inventory locally and map edited quantity
                conStore.UpdateInventory(productList);
                foreach (var product in dbStore.Inventories)
                {
                    product.Quantity = conStore.Inventory[product.Productid].Quantity;
                }
                context.SaveChanges();
                return true;
            }
            else {
                return false;
            }
        }

        public Orderr FindAnOrder(string orderid)
        {
            using var context = new Project0databaseContext(_contextOptions);
            Orderr dbOrders = context.Orderrs
                                    .Include(x => x.Orderproducts)
                                    .ThenInclude(x => x.Product)
                                    .Where(x => x.Orderid == orderid)
                                    .First();
            return dbOrders;                    
        }

        // refactor
        public bool CustomerSearch(string storeLoc,string firstName,string lastName, out string CID)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.Include(x => x.Storecustomers)
                                           .ThenInclude(x => x.Customer)
                                            .First(x => x.Storeloc == storeLoc);
            // empty store with no customer info
            var conStore = new CStore(dbStore.Storeloc, dbStore.Storephone);

            // begin
            foreach (var cust in dbStore.Storecustomers)
            {
                // extra phone number colunmn can be used for search
                CCustomer conCustomer = new CCustomer(cust.Customer.Firstname, cust.Customer.Lastname, cust.Customer.Phonenumber);
                conStore.CustomerDict[cust.Customerid] = conCustomer;
            }

            SimpleSearch ss = new SimpleSearch();
            // "" string if not found

        
            bool found = ss.SearchByName(conStore, firstName, lastName, out CID);
            if (found)
            {

                return true;
            }
            else
                return false;
        }

       
    }
}
