using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using StoreApplication.Display;
using StoreDatamodel;
using StoreLibrary;
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
            SimpleDisplay sd = new SimpleDisplay();
            SimpleSearch ss = new SimpleSearch();
    
            // 
            // enter and exit
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

                // Testing new mvc version
                Console.WriteLine("Select a store location first:");
                string storeLoc = Console.ReadLine();
                CStore store = repo.GetAStore(storeLoc);
                Console.WriteLine("\nChoose one of the following operations:\n  1.Restock 2.Add a new customer\n  3.Process an order\n  4.Search in database\n  5.Display an order\n ");
                // extra validation

                choice = Console.ReadLine();               
                if (choice == "1")
                {
                    List<CProduct> inventory = repo.GetInventoryOfAStore(storeLoc);
                    // map list to dictionary and consolidate products of the same ID
                    // product1 quantity 5,10,20 -> quantity 35
                    // clean current inventory to match data oulled
                    store.CleanInventory();
                    store.AddProducts(inventory);
                    Console.WriteLine("Inventory has been restocked");
                    // a new option to display current inventory
                    // sd.DisplayInventory();
                }
                else if (choice == "2")
                {
                    Console.WriteLine("What is the customer's first name?");
                    string firstname = Console.ReadLine();
                    Console.WriteLine("What is the customer's last name?");
                    string lastname = Console.ReadLine();
                    Console.WriteLine("What is the customer's phone number?");
                    string phonenumber = Console.ReadLine();

                    Dictionary<string, CCustomer> customers = repo.GetAllCustomersAtOneStore(storeLoc);
                    // customers have no order histroy atm
                    store.CustomerDict = customers;
                    string customerid;
                    bool Found = ss.SearchByNameAndPhone(store, firstname, lastname, phonenumber, out customerid);
                    if (Found)
                    {
                        Console.WriteLine($"Dear Customer, you already have a profile with us, here is your id {customerid}");
                    }
                    else
                    {
                        // no customer has no customerid to begin with and no order history atm
                        CCustomer newCustomer = new CCustomer(firstname, lastname, phonenumber);
                        store.AddCustomer(newCustomer);

                    }
                
                }

               















                Console.WriteLine("\nChoose one of the following operations:\n  1.Add a new customer\n  2.Process an order\n  3.Search in database\n  4.Display an order\n ");
                // extra validation

                choice = Console.ReadLine();
                if (choice == "1")
                {
                    Console.WriteLine("What is the store location?");
                    string storeLoc = Console.ReadLine();

                    Console.WriteLine("What is the customer's first name?");
                    string firstname = Console.ReadLine();
                    Console.WriteLine("What is the customer's last name?");
                    string lastname = Console.ReadLine();
                    Console.WriteLine("What is the customer's phone?");
                    string phone = Console.ReadLine();
                    repo.StoreAddACusomter(storeLoc, firstname, lastname, phone);

                    //CCustomer newCustomer = CustomerSetup(store);                                        
                    //store.AddCustomer(newCustomer);
                }
                else if (choice == "2")
                {
                    Console.WriteLine("What is the store location?");
                    string storeLoc = Console.ReadLine();

                    Console.WriteLine("What is the customer's first name?");
                    string firstname = Console.ReadLine();
                    Console.WriteLine("What is the customer's last name?");
                    string lastname = Console.ReadLine();
                    Console.WriteLine("What is the customer's phone?");
                    string phone = Console.ReadLine();

                    List<CProduct> productList = ProductsSetup();
                    bool isSuccessful = repo.CustomerPlaceAnOrder(storeLoc, firstname, lastname, phone, productList);
                    if (isSuccessful)
                    {
                        Console.WriteLine("Order placed successful");
                    }
                    else
                    {
                        Console.WriteLine("Order failed");
                    }
                    /*
                    //ISearch ss = new SimpleSearch();
                    // empty string if not found
                    string customerid;
                    bool found = ss.SearchByName(store, firstname, lastname,out customerid);
                    CCustomer customer;
                    // in order to create an order, first find the customer or create a new one
                    if (!found)
                    {
                        customer = CustomerSetup(store);
                        store.AddCustomer(customer);
                        
                    }
                    else { customer = store.CustomerDict[customerid]; }
                    */

                    /*
                    // products here
                    // probably need another while loop if an order failed
                    List<CProduct> productList = ProductsSetup();               
             
                    // exception handling here
                    // two step process: order quantity within limits -> enough inventory
                    
                    bool isSuccessful = false;
                    COrder newOrder;
                    try
                    {
                        // could contain quantity not allowed
                        newOrder = new COrder(store, customer, DateTime.Now, 100, productList);
                        isSuccessful = true;
                        Console.WriteLine("Order placed successfully!");
                    }
                    catch (ArgumentException e)
                    {
                        isSuccessful = false;
                        newOrder = new COrder();
                        Console.WriteLine("This order exceeds the max allowed quantities");
                    }

                    // failed order won't be processed
                    if (isSuccessful)
                    {
                        if (store.CheckInventory(newOrder))
                        {
                            customer.PlaceOrder(store, newOrder);
                            Console.WriteLine("Order placed successfullly");
                        }
                        else
                        {
                            Console.WriteLine("Not enough quantity to fullfill");
                        }
                    }
                    */
                }

                else if (choice == "3")
                {
                    Console.WriteLine("Enter the store location");
                    string storeLoc = Console.ReadLine();

                    Console.WriteLine("Enter Customer's first name");
                    string firstname = Console.ReadLine();
                    Console.WriteLine("Enter Customer's last name");

                    string lastname = Console.ReadLine();
                    string CID;
                    bool found = repo.CustomerSearch(storeLoc, firstname, lastname,out CID);
                    if (found)
                        Console.WriteLine($"CustomerID: {CID} found, customer alreay exist in the database");
                    else
                        Console.WriteLine("Customer not found, proceed to option 1 to create a new profile");
                    /*
                    SimpleSearch ss = new SimpleSearch();
                    // empty string if not found
                    string customerid;
                    bool found = ss.SearchByName(store, firstname, lastname, out customerid);
                    if (found)
                    {
                        Console.WriteLine("customer profile does not exist");
                    }
                    else { Console.WriteLine("customer profile found"); }
                    */
                }
                else if (choice == "4")
                {
                    Console.WriteLine("What is the orderid?");
                    string orderid = Console.ReadLine();
                    sd.DisplayOneOrder(repo.FindAnOrder(orderid));
                }

                    /*
                    IDisplay sd = new SimpleDisplay();
                    Console.WriteLine("1 Display an order\t 2 Display all orders of a customer\t 3 Display all orders of the store");
                    int input = Console.Read();
                    if (input == 1)
                    {
                        Console.WriteLine("Enter the customer'id who placed the order");
                        string customerid = Console.ReadLine();
                        Console.WriteLine("Enter the order number");
                        string orderid = Console.ReadLine();
                        // exception cutomerid or orderid not found
                        foreach (var order in store.CustomerDict[customerid].OrderHistory)
                        {
                            if (order.Orderid == orderid)
                            {
                                sd.DisplayOneOrder(order);
                            }
                        }
                        // no display means customerid or orderif typed wrong
                    }
                    else if (input == 2)
                    {
                        Console.WriteLine("Enter the customer'id who placed the order");
                        string customerid = Console.ReadLine();
                        sd.DisplayAllOrders(store.CustomerDict[customerid].OrderHistory);
                    }
                    else if (input == 3)
                    {
                        foreach (var pair in store.CustomerDict)
                        {
                            sd.DisplayAllOrders(pair.Value.OrderHistory);
                        }
                    }
                    */ 
            } 
        }

        
        /*
        private static CCustomer CustomerSetup(CStore store)
        {
            Console.WriteLine("What is the customer id?");
            string customerid = Console.ReadLine();
            Console.WriteLine("What is the customer's first name?");
            string firstname = Console.ReadLine();
            Console.WriteLine("What is the customer's last name?");
            string lastname = Console.ReadLine();
            Console.WriteLine("What is the customer's phone?");    
            string phone = Console.ReadLine();

            CCustomer newCustomer = new CCustomer(customerid, firstname, lastname, phone);
            return newCustomer;
        }
        */


        private static List<CProduct> ProductsSetup()
        {
            List<CProduct> productList = new List<CProduct>();
            Console.WriteLine("Setting up products, enter any key to continue, enter 'x' to exit");
            string init = Console.ReadLine();
            bool hasNext = true;
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

