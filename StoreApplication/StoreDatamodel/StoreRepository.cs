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

        // create a dict of products that can be added to a given store
        public List<CProduct> GetInventoryOfAStore(string storeLoc)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.Include(x => x.Inventories)
                                            .ThenInclude(x => x.Product)
                                                .First(x => x.Storeloc == storeLoc);
            List<CProduct> inventory = new List<CProduct>();
            foreach (var product in dbStore.Inventories)
            {
                CProduct p = new CProduct(product.Product.Productid, product.Product.Name, 
                                            product.Product.Category, product.Product.Price, product.Quantity);
                inventory.Add(p);
            }
            return inventory;
        }

        // create a dictionary of customer to be added to a given store
        public Dictionary<string, CCustomer> GetAllCustomersAtOneStore(string storeLoc)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.Include(x => x.Storecustomers)
                                            .ThenInclude(x => x.Customer)
                                                .First(x => x.Storeloc == storeLoc);
            Dictionary<string, CCustomer> customers = new Dictionary<string, CCustomer>();
            foreach (var customer in dbStore.Storecustomers)
            {
                CCustomer c = new CCustomer(customer.Customer.Customerid, customer.Customer.Firstname,
                                                customer.Customer.Lastname, customer.Customer.Phonenumber);
                // these customers have no order history atm
                customers[c.Customerid] = c;
            }
            return customers;
        }

        // create a list of order for a customer
        public List<COrder> GetAllOrdersOfOneCustomer(string customerid, CStore store, CCustomer customer)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCustomer = context.Customers.Include(x => x.Orderrs).First(x => x.Customerid == customerid);

            List<COrder> orders = new List<COrder>();
            foreach (var order in dbCustomer.Orderrs)
            {
                // these orders have no product list
                // total cost set to 0 for now
                COrder o = new COrder(order.Orderid, store, customer, DateTime.Now,0);
                orders.Add(o);
            }

            return orders;

        }

        // create a list of products for an order
        public List<CProduct> GetAllProductsOfOneOrder(string orderid)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbOrder = context.Orderrs.Include(x => x.Orderproducts)
                                            .ThenInclude(x => x.Product)
                                                .First(x => x.Orderid == orderid);
            List<CProduct> products = new List<CProduct>();
            foreach (var product in dbOrder.Orderproducts)
            {
                CProduct p = new CProduct(product.Product.Productid, product.Product.Name, product.Product.Category,
                                            product.Product.Price, product.Quantity);
                products.Add(p);
            }
            return products;
       }

        public List<CStore> GetAllStores()
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStores = context.Stores.ToList();
            List<CStore> stores = new List<CStore>();
            foreach (var store in dbStores)
            {
                CStore s = new CStore(store.Storeloc, store.Storephone);
                stores.Add(s);
            }
            return stores;
        }

        public void StoreAddOneCusomter(string storeLoc, CCustomer customer)
        {
            using var context = new Project0databaseContext(_contextOptions);
            // only have this part below in the data model, rest moves to console main
            var newCustomer = new Customer {
                Customerid = customer.Customerid,
                Firstname = customer.FirstName,
                Lastname = customer.LastName,
                Phonenumber = customer.PhoneNumber
            };
            context.Customers.Add(newCustomer);       
            context.SaveChanges();

            // many to many, bridge table gets updated as well
            var newBridge = new Storecustomer
            {
                Storeloc = storeLoc,
                Customerid = customer.Customerid
            };
            context.Storecustomers.Add(newBridge);
            context.SaveChanges();
            }            
        }




        // same changes, only keep the part that updates tables, move others to class model or console main
        public bool CustomerPlaceAnOrder(string storeLoc, string firstName, string lastName,string phoneNumber,List<CProduct> productList)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.Include(x => x.Inventories)
                                                 .ThenInclude(x => x.Product)
                                                   .First(x => x.Storeloc == storeLoc);
            

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
