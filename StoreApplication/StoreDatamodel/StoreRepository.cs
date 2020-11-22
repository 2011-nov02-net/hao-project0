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
    public class StoreRepository : IStoreRepository
    {
        private readonly DbContextOptions<Project0databaseContext> _contextOptions;
        public StoreRepository(DbContextOptions<Project0databaseContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }       

        // M V C design
        // re-implementation seperating business and data-access
        // create a default store with no customer profile and inventory    

        public CStore GetAStore(string storeLoc)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.FirstOrDefault(x => x.Storeloc == storeLoc);
            if (dbStore == null) return null;
            // store has no customer profile yet
            CStore store = new CStore(dbStore.Storeloc, dbStore.Storephone);
            return store;
        }

        // create a dict of products that can be added to a given store
        public List<CProduct> GetInventoryOfAStore(string storeLoc)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.Include(x => x.Inventories)
                                            .ThenInclude(x => x.Product)
                                                .FirstOrDefault(x => x.Storeloc == storeLoc);
            if (dbStore == null) return null;
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
                                                .FirstOrDefault(x => x.Storeloc == storeLoc);
            if (dbStore == null) return null;
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
            var dbCustomer = context.Customers.Include(x => x.Orderrs).FirstOrDefault(x => x.Customerid == customerid);
            if (dbCustomer == null) return null;
            List<COrder> orders = new List<COrder>();
            foreach (var order in dbCustomer.Orderrs)
            {
                // these orders have no product list
                // total cost not yet set
                COrder o = new COrder(order.Orderid, store, customer, DateTime.Now);
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
                                                .FirstOrDefault(x => x.Orderid == orderid);
            if (dbOrder == null) return null;
            List<CProduct> products = new List<CProduct>();
            foreach (var product in dbOrder.Orderproducts)
            {
                CProduct p = new CProduct(product.Product.Productid, product.Product.Name, product.Product.Category,
                                            product.Product.Price, product.Quantity);
                products.Add(p);
            }
            return products;
        }

        
        // all functionalities
        public void StoreAddOneCusomter(string storeLoc, CCustomer customer)
        {
            using var context = new Project0databaseContext(_contextOptions);
            // only have this part below in the data model, rest moves to console main
            var newCustomer = new Customer
            {
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

        // same changes, only keep the part that updates tables, move others to class model or console main
        public void CustomerPlaceOneOrder(COrder order, CStore store, double totalCost)
        {
            using var context = new Project0databaseContext(_contextOptions);
            // update order
            var newOrder = new Orderr
            {
                Orderid = order.Orderid,
                Storeloc = order.StoreLocation.Storeloc,
                Customerid = order.Customer.Customerid,
                Orderedtime = DateTime.Now,
                Totalcost = totalCost
            };
            context.Orderrs.Add(newOrder);
            context.SaveChanges();

            // update Orderproduct  
            foreach (var product in order.ProductList)
            {
                var newOP = new Orderproduct
                {
                    Orderid = order.Orderid,
                    Productid = product.UniqueID,
                    Quantity = product.Quantity
                };
                context.Orderproducts.Add(newOP);
            }
            context.SaveChanges();

            var dbStore = context.Stores.Include(x => x.Inventories)
                                                .FirstOrDefault(x => x.Storeloc == order.StoreLocation.Storeloc);
            // if (dbStore == null) return null;
            // update inventory quantity          
            foreach (var product in order.ProductList)
            {
                foreach (var dbProd in dbStore.Inventories)
                {
                    if (product.UniqueID == dbProd.Productid)
                    {
                        dbProd.Quantity = store.Inventory[product.UniqueID].Quantity;
                    }
                }
            }
            context.SaveChanges();

        }
        // simply search for a customer       
        public CCustomer GetOneCustomerByNameAndPhone(string firstName, string lastName, string phonenumber)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCustomer = context.Customers
                             .FirstOrDefault(x => x.Firstname == firstName && x.Lastname == lastName && x.Phonenumber == phonenumber);
            if (dbCustomer == null) return null;
            CCustomer foundCustomer;
            foundCustomer = new CCustomer(dbCustomer.Customerid,
                                                    dbCustomer.Firstname, dbCustomer.Lastname, dbCustomer.Phonenumber);          
            return foundCustomer;
        }

        public COrder GetAnOrderByID(string orderid)
        {
            using var context = new Project0databaseContext(_contextOptions);
            Orderr dbOrder = context.Orderrs
                                    .Include(x => x.Orderproducts)
                                        .ThenInclude(x => x.Product)
                                        .FirstOrDefault(x => x.Orderid == orderid);
            Orderr dbCustomer = context.Orderrs.Include(x => x.Customer).FirstOrDefault(x => x.Orderid == orderid);
            if (dbOrder == null) return null;
            if (dbCustomer == null) return null;

            COrder order = new COrder(orderid, new CStore(dbOrder.Storeloc),
                                               new CCustomer(dbCustomer.Customer.Firstname, dbCustomer.Customer.Lastname,
                                                             dbCustomer.Customer.Phonenumber),
                                               dbOrder.Orderedtime, dbOrder.Totalcost);

            foreach (var product in dbOrder.Orderproducts)
            {
                CProduct p = new CProduct(product.Product.Productid, product.Product.Name,
                                        product.Product.Category, product.Product.Price, product.Quantity);
                order.ProductList.Add(p);
            }
            return order;
        }
        // find all detail of a customer
        public CCustomer GetOneCustomerOrderHistory(string firstName, string lastName, string phoneNumber, CStore store)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCustomer = context.Customers
                             .FirstOrDefault(x => x.Firstname == firstName && x.Lastname == lastName && x.Phonenumber == phoneNumber);
            if (dbCustomer == null) return null;
            CCustomer foundCustomer;
            foundCustomer = new CCustomer(dbCustomer.Customerid,
                                                    dbCustomer.Firstname, dbCustomer.Lastname, dbCustomer.Phonenumber);

            foundCustomer.OrderHistory = GetAllOrdersOfOneCustomer(foundCustomer.Customerid, store, foundCustomer);
            foreach (var order in foundCustomer.OrderHistory)
            {
                order.ProductList = GetAllProductsOfOneOrder(order.Orderid);
                order.TotalCost = store.CalculateTotalPrice(order.ProductList);
            }

            return foundCustomer;
        }

        public CStore GetOneStoreOrderHistory(string storeLoc)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.FirstOrDefault(x => x.Storeloc == storeLoc);
            if (dbStore == null) return null;
            // store has no customer profile yet
            CStore seekStore = new CStore(dbStore.Storeloc, dbStore.Storephone);
            seekStore.CustomerDict = GetAllCustomersAtOneStore(storeLoc);

            foreach (var customer in seekStore.CustomerDict)
            {
                CCustomer cust = customer.Value;
                cust.OrderHistory = GetAllOrdersOfOneCustomer(cust.Customerid, seekStore, cust);
                foreach (var order in cust.OrderHistory)
                {
                    order.ProductList = GetAllProductsOfOneOrder(order.Orderid);
                    order.TotalCost = seekStore.CalculateTotalPrice(order.ProductList);
                }
            }
            return seekStore;
        }

        // add methods
        public void AddOneStore(CStore store)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var newStore = new Store
            {
                Storeloc = store.Storeloc,
                Storephone = store.Storephone
            };
            context.Stores.Add(newStore);
            context.SaveChanges();

        }
       
        public void AddOneProduct(CProduct product)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var newProduct = new Product
            {
                Productid = product.UniqueID,
                Name = product.Name,
                Category = product.Category,
                Price = product.Price
            };
            context.Products.Add(newProduct);
            context.SaveChanges();

        }

        // helpers
        public List<CStore> GetAllStores()
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStores = context.Stores.ToList();
            if (dbStores == null) return null;
            List<CStore> stores = new List<CStore>();
            foreach (var store in dbStores)
            {
                CStore s = new CStore(store.Storeloc, store.Storephone);
                stores.Add(s);
            }
            return stores;
        }

        public IEnumerable<CProduct> GetAllProducts()
        {
            using var context = new Project0databaseContext(_contextOptions);
            IEnumerable<Product> dbProducts = context.Products.ToList();
            IEnumerable<CProduct> conProducts = dbProducts.Select(x => new CProduct(x.Productid, x.Name, x.Category, x.Price, 1));
            return conProducts;
        }

        public CProduct GetAProductByNameAndCategory(string name, string category)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbProduct = context.Products.FirstOrDefault(x => x.Name == name && x.Category == category);
            if (dbProduct == null) return null;
            CProduct p = new CProduct(dbProduct.Productid, dbProduct.Name, dbProduct.Category, dbProduct.Price);

            return p;
        }
    }
}


        

