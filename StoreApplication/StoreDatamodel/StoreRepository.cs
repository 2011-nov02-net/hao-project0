﻿using Microsoft.EntityFrameworkCore;
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
            // did not calculate total
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

        // same changes, only keep the part that updates tables, move others to class model or console main
        public void CustomerPlaceOneOrder(COrder order, CStore store, double totalCost )
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
                                                .First(x => x.Storeloc == order.StoreLocation.Storeloc);
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

        // refactor
        public CCustomer GetOneCustomerByNameAndPhone(string firstName,string lastName, string phonenumber)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCustomer = context.Customers
                             .FirstOrDefault(x => x.Firstname == firstName && x.Lastname == lastName && x.Phonenumber == phonenumber);
            CCustomer foundCustomer;
            if (dbCustomer != null)
            {
                foundCustomer = new CCustomer(dbCustomer.Customerid,
                                                    dbCustomer.Firstname, dbCustomer.Lastname, dbCustomer.Phonenumber);
            }
            else
            {
                foundCustomer = new CCustomer();
            }
                return foundCustomer;  
        }

        public COrder GetAnOrderByID(string orderid)
        {
            using var context = new Project0databaseContext(_contextOptions);
            Orderr dbOrder = context.Orderrs
                                    .Include(x => x.Orderproducts)
                                        .ThenInclude(x => x.Product)
                                        .First(x => x.Orderid == orderid);
            COrder order = new COrder(orderid, new CStore(dbOrder.Storeloc),
                                               new CCustomer(dbOrder.Customer.Customerid, dbOrder.Customer.Firstname,
                                                            dbOrder.Customer.Lastname, dbOrder.Customer.Phonenumber),
                                               dbOrder.Orderedtime, dbOrder.Totalcost);
            
            foreach (var product in dbOrder.Orderproducts)
            {
                CProduct p = new CProduct(product.Product.Productid, product.Product.Name,
                                        product.Product.Category, product.Product.Price, product.Quantity);
                order.ProductList.Add(p);
            }

            return order;
        }


        public 


    }
}
