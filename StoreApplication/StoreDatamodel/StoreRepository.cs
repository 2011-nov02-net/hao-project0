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
    public class StoreRepository
    {
        private readonly DbContextOptions<Project0databaseContext> _contextOptions;
        public StoreRepository(DbContextOptions<Project0databaseContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }

        // all functions required
        // pull data from db and create CProduct objects
        // default getters like product, customer, order, store

        // all in product table
        public IEnumerable<CProduct> GetAllProducts()
        {
            using var context = new Project0databaseContext(_contextOptions);
            IEnumerable<Product> dbProducts = context.Products.ToList();
            IEnumerable<CProduct> conProducts = dbProducts.Select(x => new CProduct(x.Productid, x.Name, x.Category, x.Price, 1));
            return conProducts;
        }

        // join product and orderproduct
        public CProduct GetFirstProductById(string productid)
        {
            using var context = new Project0databaseContext(_contextOptions);
            Product dbProduct = context.Products
                                        .Include(c => c.Orderproducts)
                                        .First(c => c.Productid == productid);

            CProduct conProduct = new CProduct(dbProduct.Productid, dbProduct.Name, dbProduct.Category, dbProduct.Price, dbProduct.Orderproducts.First().Quantity);
            return conProduct;
        }


        public List<CProduct> GetAllProductsById(string productid)
        {
            using var context = new Project0databaseContext(_contextOptions);
            Product dbProduct = context.Products
                                    .Include(c => c.Orderproducts)
                                    .First(c => c.Productid == productid);

            List<CProduct> conProducts = new List<CProduct>();

            foreach (var product in dbProduct.Orderproducts)
            {
                CProduct conProduct = new CProduct(dbProduct.Productid, dbProduct.Name, dbProduct.Category, dbProduct.Price, product.Quantity);
                conProducts.Add(conProduct);
            }
            return conProducts;
        }


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
           
            SimpleSearch ss = new SimpleSearch();
            // "" string if not found
            
            string CID = "";
            bool found = ss.SearchByName(conStore, firstName, lastName, out CID);
            if (found)
            {
                Console.WriteLine("already in");
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
                Console.WriteLine("just added a new customer");
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
                Console.WriteLine(product.Productid, product.Product.Name, product.Product.Category, product.Product.Price, product.Quantity);           
                inventory.Add(p);
               
            }

            // dictionary "productid":product(contains quantity)
            conStore.AddProducts(inventory);
            Console.WriteLine("inventory created");

            foreach (var pair in conStore.Inventory)
            {
                Console.WriteLine(pair.Key, pair.Value.UniqueID, pair.Value.Quantity);
            }


            // check customer's quantity against inventory quantity
            bool isEnough = conStore.CheckInventory(productList);
            string customerid;
            if (isEnough)
            {
                // this probably won't work because of new db context
                Console.WriteLine("about to add a customer");
                customerid = StoreAddACusomter(storeLoc, firstName, lastName, phoneNumber);
                // Customer table and Storecustomer table updated

                string orderid = OIDGen.Gen();
                // update order
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
                Console.WriteLine("order updated");

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
                Console.WriteLine("orderproduct updated");


                // update inventory locally
                conStore.UpdateInventory(productList);
                // map updated numbers

                // edit inventory
                foreach (var product in dbStore.Inventories)
                {
                    product.Quantity = conStore.Inventory[product.Productid].Quantity;
                }
                context.SaveChanges();
                Console.WriteLine("Inventroy updated");

                return true;


            }

            else {
                // not enough inventory
                return false;
            }




        }

    }
}
