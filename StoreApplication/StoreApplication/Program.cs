using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using StoreApplication.Display;
using StoreDatamodel;
using StoreLibrary;
using StoreLibrary.IDGenerator;
using StoreLibrary.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using System.Xml;


namespace StoreApplication
{
    class Program
    {      
        static DbContextOptions<Project0databaseContext> s_dbContextOptions;

        static void Main(string[] args)
        {
            using var logStream = new StreamWriter("ef-logs.txt");
            var optionsBuilder = new DbContextOptionsBuilder<Project0databaseContext>();
            optionsBuilder.UseSqlServer(GetConnectionString());
            optionsBuilder.LogTo(logStream.WriteLine, LogLevel.Debug);
            s_dbContextOptions = optionsBuilder.Options;

            StoreRepository repo = new StoreRepository(s_dbContextOptions);
            IDisplay sd = new SimpleDisplay();
            ISearch ss = new SimpleSearch();

            Console.WriteLine("Welcome to XYZ Enterprise, enter any key to continue, 'x' to exit");

            // all inputs have a validation method to process Ivalid inputs
            // to do
            string choice = Console.ReadLine();
            while (choice != "x")
            {
                // read from databse
                // string path = "../../../SimplyWriteData.json";
                // JsonFilePersist persist = new JsonFilePersist(path);
                // CStore store = persist.ReadStoreData();

                // new mvc version
                Console.WriteLine("Select a store location first:");
                string storeLoc = Console.ReadLine();
                CStore store = repo.GetAStore(storeLoc);

                // set up inventory, not the same as add products, more like reset inventory
                InventorySetup(repo, storeLoc, store);
                Console.WriteLine("\nChoose one of the following operations:\n  1.Add a new customer\n  2.Process an order\n  3.Search a customer\n  4.Display an order ");
                // validation

                bool hasProfileSetup = false;
                choice = Console.ReadLine();
                if (choice == "1")
                {
                    // avoid repetition if already has all customer profiles
                    if (!hasProfileSetup)
                    {
                        CustomerSetup(repo, storeLoc, store);
                        hasProfileSetup = true;
                    }
                    // add a new customer profile if not exist, or nothing
                    CheckAndAddOneCustomer(repo, storeLoc, store, ss);
                }
                else if (choice == "2")
                {
                    // same process as choiece 2 in the beginning
                    if (!hasProfileSetup)
                    {
                        CustomerSetup(repo, storeLoc, store);
                        hasProfileSetup = true;
                    }
                    string customerid = CheckAndAddOneCustomer(repo, storeLoc, store, ss);

                    // process begins
                    List<CProduct> products = ProductsSetup();
                    string orderid = OIDGen.Gen();
                    double totalCost = store.CalculateTotalPrice(products);
                    bool isSuccessful = false;
                    COrder newOrder = new COrder(orderid, store, store.CustomerDict[customerid],
                                                DateTime.Now, totalCost);
                    try
                    {
                        // quantity limits
                        newOrder.ProductList = products;
                        isSuccessful = true;
                        Console.WriteLine("Order created successfully!");
                    }
                    catch (ArgumentException e)
                    {
                        isSuccessful = false;
                        Console.WriteLine("This order exceeds the max allowed quantities, failed to create the order!");
                    }

                    if (isSuccessful)
                    {
                        if (!store.CheckInventory(newOrder))
                        {
                            Console.WriteLine("Do not have enough products left to fulfill this order!");
                        }
                        else
                        {
                            // map products to an order, orders to a customer,
                            // store now has complete information
                            foreach (var pair in store.CustomerDict)
                            {
                                CCustomer customer = pair.Value;
                                customer.OrderHistory = repo.GetAllOrdersOfOneCustomer(customer.Customerid, store, customer);
                                foreach (var order in customer.OrderHistory)
                                {
                                    order.ProductList = repo.GetAllProductsOfOneOrder(order.Orderid);
                                    order.TotalCost = store.CalculateTotalPrice(order.ProductList);
                                }

                            }
                            store.UpdateInventoryAndCustomerOrder(newOrder);
                            repo.CustomerPlaceOneOrder(newOrder, store, totalCost);
                        }
                    }
                }

                else if (choice == "3")
                {
                    Console.WriteLine("Enter Customer's first name");
                    string firstname = Console.ReadLine();
                    Console.WriteLine("Enter Customer's last name");
                    string lastname = Console.ReadLine();
                    Console.WriteLine("Enter Customer's phone number");
                    string phonenumber = Console.ReadLine();

                    CCustomer foundCustomer = repo.GetOneCustomerByNameAndPhone(firstname, lastname, phonenumber);
                    if (foundCustomer.Customerid != null)
                        Console.WriteLine($"{foundCustomer.Customerid} found, customer alreay exist in the database");
                    else
                        Console.WriteLine("Customer not found, proceed to option 1 to create a new profile");
                }
                else if (choice == "4")
                {
                    Console.WriteLine("What is the orderid?");
                    string orderid = Console.ReadLine();
                    sd.DisplayOneOrder(repo.GetAnOrderByID(orderid));
                }
                else if (choice == "5")
                {
                    // search for a customer
                    Console.WriteLine("Enter Customer's first name");
                    string firstname = Console.ReadLine();
                    Console.WriteLine("Enter Customer's last name");
                    string lastname = Console.ReadLine();
                    Console.WriteLine("Enter Customer's phone number");
                    string phonenumber = Console.ReadLine();
                    CCustomer foundCustomer = repo.GetOneCustomerByNameAndPhone(firstname, lastname, phonenumber);
                    // 
                    if (foundCustomer.Customerid != null)
                        sd.DisplayAllOrders(foundCustomer.OrderHistory);
                    else
                        Console.WriteLine("Customer not found, proceed to option 1 to create a new profile");
                }
                else if (choice == "6")
                {
                    Console.WriteLine("What is the store location you seek?");
                    string seekLoc = Console.ReadLine();
                    CStore seekStore = repo.GetAStore(seekLoc);
                    seekStore.CustomerDict = repo.GetAllCustomersAtOneStore(seekLoc);

                    foreach (var pair in seekStore.CustomerDict)
                    {
                        CCustomer cust = pair.Value;
                        cust.OrderHistory = repo.GetAllOrdersOfOneCustomer(cust.Customerid, seekStore, cust);
                        foreach (var order in cust.OrderHistory)
                        {
                            order.ProductList = repo.GetAllProductsOfOneOrder(order.Orderid);
                            order.TotalCost = store.CalculateTotalPrice(order.ProductList);
                        }
                        sd.DisplayAllOrders(cust.OrderHistory);
                    }

                   
                }

            }
        }


        // helper classes refactored        
        private static void InventorySetup(IStoreRepository repo, string storeLoc, CStore store)
        {
            List<CProduct> inventory = repo.GetInventoryOfAStore(storeLoc);           
            store.CleanInventory();
            store.AddProducts(inventory);
            Console.WriteLine("Initial inventory set up done");
        }

        private static void CustomerSetup(IStoreRepository repo, string storeLoc, CStore store)
        {
            Dictionary<string, CCustomer> customers = repo.GetAllCustomersAtOneStore(storeLoc);
            store.CustomerDict = customers;
            Console.WriteLine("Initial customer profile set up done");
        }


        private static string CheckAndAddOneCustomer(StoreRepository repo,string storeLoc,CStore store,ISearch ss)
        {
            // validation
            Console.WriteLine("What is the customer's first name?");
            string firstname = Console.ReadLine();
            Console.WriteLine("What is the customer's last name?");
            string lastname = Console.ReadLine();
            Console.WriteLine("What is the customer's phone number?");
            string phonenumber = Console.ReadLine();

            string customerid;
            // by name or by name and phone
            bool Found = ss.SearchByNameAndPhone(store, firstname, lastname, phonenumber, out customerid);
            if (Found)
            {
                Console.WriteLine($"Dear Customer, you already have a profile with us, here is your customer id {customerid}");
            }
            else
            {
                // new customer has no order history atm
                customerid = CIDGen.Gen();
                CCustomer newCustomer = new CCustomer(customerid, firstname, lastname, phonenumber);
                store.AddCustomer(newCustomer);
                repo.StoreAddOneCusomter(storeLoc, newCustomer);
                Console.WriteLine($"Dear {customerid}, your profile has been set up successfuly");
            }
            return customerid;
        }

        private static List<CProduct> ProductsSetup()
        {
            List<CProduct> productList = new List<CProduct>();
            Console.WriteLine("Setting up products, enter any key to continue, enter 'x' to exit");
            string init = Console.ReadLine();
            while (init != "x")
            {
                Console.WriteLine("Enter Product unique ID");
                string id = Console.ReadLine();

                Console.WriteLine("Enter Product name");
                string name = Console.ReadLine();

                Console.WriteLine("Enter Product category");
                string category = Console.ReadLine();

                Console.WriteLine("Enter Product price");
                string priceStr = Console.ReadLine();
                double price;
                Double.TryParse(priceStr, out price);

                Console.WriteLine("Enter Product quantity");
                string quantityStr = Console.ReadLine();
                int quantity;
                int.TryParse(quantityStr, out quantity);

                CProduct p = new CProduct(id, name, category, price, quantity);
                productList.Add(p);

                Console.WriteLine("Press x to end");
                init = Console.ReadLine();
            }
            return productList;
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

