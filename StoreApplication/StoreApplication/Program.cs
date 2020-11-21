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
                // display all store locations to choose from
                List<CStore> storeBasics = repo.GetAllStores();
                sd.DisplayAllStores(storeBasics);

                Console.WriteLine("Select a store location first:");
                string storeLoc = Console.ReadLine();
                CStore store = null;
                while (store == null)
                {                   
                    store = repo.GetAStore(storeLoc);
                    if (NullChecker(store))
                    {
                        storeLoc = Console.ReadLine();
                        continue;
                    }
                    else break;                     
                }
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
                    List<CProduct> products = ProductsSetup(repo);
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
                    while (true)
                    {
                        Console.WriteLine("Enter Customer's first name");
                        string firstname = ValidateNotNull(Console.ReadLine());
                        Console.WriteLine("Enter Customer's last name");
                        string lastname = ValidateNotNull(Console.ReadLine());
                        Console.WriteLine("Enter Customer's phone number");
                        string phonenumber = ValidatePhonenumber(Console.ReadLine());

                        CCustomer foundCustomer = repo.GetOneCustomerByNameAndPhone(firstname, lastname, phonenumber);
                        if (NullChecker(foundCustomer)) continue;
                        else
                        {
                            Console.WriteLine($"{foundCustomer.Customerid} found, customer alreay exist in the database");
                            break;
                        }
                    }
                }
                else if (choice == "4")
                {
                    while (true)
                    {
                        Console.WriteLine("What is the orderid?");
                        string orderid = ValidateNotNull(Console.ReadLine());
                        COrder foundOrder = repo.GetAnOrderByID(orderid);
                        if (NullChecker(foundOrder)) continue;
                        else
                        {
                            sd.DisplayOneOrder(foundOrder);
                            break;
                        }
                        // need an option to search by store location, customerid, and orderdata
                    }
                }
                else if (choice == "5")
                {
                    while (true)
                    {
                        // same as search for a customer in the beginning
                        Console.WriteLine("Enter Customer's first name");
                        string firstname = ValidateNotNull(Console.ReadLine());
                        Console.WriteLine("Enter Customer's last name");
                        string lastname = ValidateNotNull(Console.ReadLine());
                        Console.WriteLine("Enter Customer's phone number");
                        string phonenumber = ValidatePhonenumber(Console.ReadLine());
                        CCustomer foundCustomer = repo.GetOneCustomerByNameAndPhone(firstname, lastname, phonenumber);

                        if (NullChecker(foundCustomer)) continue;
                        else
                        {
                            sd.DisplayAllOrders(foundCustomer.OrderHistory);
                            break;
                        }
                    }
                }
                else if (choice == "6")
                {
                    while (true)
                    {
                        Console.WriteLine("What is the store location you seek?");
                        string seekLoc = ValidateNotNull(Console.ReadLine());
                        CStore seekStore = repo.GetAStore(seekLoc);
                        if (NullChecker(seekStore)) continue;

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
                // invalid commands
                else
                {
                    Console.WriteLine("Choose one of the options above, other inputs are invalid!");
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
            
            Console.WriteLine("What is the customer's first name?");
            string firstname = ValidateNotNull(Console.ReadLine());
            Console.WriteLine("What is the customer's last name?");
            string lastname = ValidateNotNull(Console.ReadLine());
            Console.WriteLine("What is the customer's phone number?");         
            string phonenumber = ValidatePhonenumber(Console.ReadLine());
            string customerid;
            // or use repo.GetOneCustomerByNameAndPhone, check null reference
            // can delay setting up customer profiles
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

        private static List<CProduct> ProductsSetup(StoreRepository repo)
        {
            List<CProduct> productList = new List<CProduct>();
            Console.WriteLine("Puchasing products, enter any key to continue, enter 'x' to exit");
            string init = Console.ReadLine();
            while (init != "x")
            {                 
                Console.WriteLine("Enter Product name");
                string name = ValidateNotNull(Console.ReadLine());
                Console.WriteLine("Enter Product category");
                string category = ValidateNotNull(Console.ReadLine());
                /*
                Console.WriteLine("Enter Product price");
                string priceStr = Console.ReadLine();
                double price;
                Double.TryParse(priceStr, out price);
                */
                Console.WriteLine("Enter Product quantity");
                int quantity = ValidateInt(Console.ReadLine());               
               
                CProduct p = repo.GetAProductByNameAndCategory(name, category);
                if (NullChecker(p)) continue;
                
                p.Quantity = quantity;
                productList.Add(p);

                Console.WriteLine("Press any key to continue, and 'x' to end");
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

        // check object created from db
        private static bool NullChecker(Object obj)
        {
            if (obj == null)
            {
                Console.WriteLine("Record not found, make sure your input matches!");
                return true;
            }
            else return false;   
        }


        // validate attributes that cannot be null
        private static string ValidateNotNull(string input)
        {
            while (true)
            {
                if (input == null || input == "")
                {
                    Console.WriteLine("Cannot be empty");
                    input = Console.ReadLine();
                    continue;
                }
                else
                {
                    return input;
                }
            }
        }

        // validate phone number , not null, len = 10, numbers only
        private static string ValidatePhonenumber(string phoneNumber)
        {
            while (true)
            {
                if (phoneNumber == null || phoneNumber == "")
                {
                    Console.WriteLine("Cannot be empty");
                    phoneNumber = Console.ReadLine();
                    continue;
                }
                else
                {
                    if (phoneNumber.Length == 10)
                    {
                        // need to have regular expression
                        // can only contain numbers
                        return phoneNumber;

                    }
                    else
                    {
                        Console.WriteLine("Not enough digits");
                        phoneNumber = Console.ReadLine();
                        continue;
                    }

                    
                }
            }
        }

        // validate quantity, not null, > 0
        private static int ValidateInt(string number)
        {
            while (true)
            {
                if (number == null || number == "")
                {
                    Console.WriteLine("Cannot be empty");
                    number = Console.ReadLine();
                    continue;
                }
                else
                {
                    int value = 0;
                    int.TryParse(number, out value);
                    if (value > 0)
                    {
                        return value;
                    }
                    else
                    {
                        Console.WriteLine("Value must be positive");
                        number = Console.ReadLine();
                        continue;
                    }
                }
            }
            
             
        }

        // validate price, not null, > 0
        // not used
        private static double ValidateDouble(string number)
        {
            while (true)
            {
                if (number == null || number == "")
                {
                    Console.WriteLine("Cannot be empty");
                    number = Console.ReadLine();
                    continue;
                }
                else
                {
                    double value = 0;
                    double.TryParse(number, out value);
                    if (value > 0)
                    {
                        return value;
                    }
                    else
                    {
                        Console.WriteLine("Value must be positive");
                        number = Console.ReadLine();
                        continue;
                    }
                }
            }
        }

    }
}

