using StoreApplication.Display;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Xml;


namespace StoreApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // enter and exit
            Console.WriteLine("Welcome to XYZ Enterprise, enter any key to continue, 'x' to exit");

            // all inputs have a validation method to process IValid
            // to do
            string init = Console.ReadLine()
            while (init != "x")
            {
                // read from databse
                string path = "../../../SimplyWriteData.json";
                JsonFilePersist persist = new JsonFilePersist(path);
                Store store = persist.ReadStoreData();

                Console.WriteLine("1.Add a new customer\n  2.Process an order\n  3.Restock\n  4.Search in database\n  5.Display order details\n ");
                // extra validation
                int choice = Console.Read();
                if (choice == 1)
                {
                    Customer newCustomer = CustomerSetup(store);
                    // Console.WriteLine("What is the customer's default store?");
                    // future implementation, search a location with unique branchID/address
                    // string branchID = Console.ReadLine();
                    // use the default store for now                         
                    store.AddCustomer(newCustomer);
                }
                else if (choice == 2)
                {
                    // in order to add an order, first find a customer
                    Customer customer = CustomerSetup(store);
                    List<IProduct> productList = ProductsSetup();
                    // location, customer, orderedtime, productList
                    // create an order
                    Order newOrder = new Order(store, customer, DateTime.Now, productList);
                    if (store.CheckInventory(newOrder))
                    {
                        try
                        {
                            customer.PlaceOrder(store, newOrder);
                            Console.WriteLine("Order placed successfullly");
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine("This order contains too many products!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Not enough quantity to fullfill");
                    }
                }


                else if (choice == 3)
                {
                    List<IProduct> supply = ProductsSetup();
                    // same process to key in product details in a loop                    
                    store.AddProducts(supply);
                }

                else if (choice == 4)
                {
                    Console.WriteLine("Enter Customer's Name");
                    // use search function to go through store.UserDict
                    // Console.WriteLine
                }
                else if (choice == 5)
                { 
                    // use display function 
                }

            }
        }


        private static Customer CustomerSetup(Store store)
        {
            Console.WriteLine("What is the customer's SSN?");

            string social = Console.ReadLine();
            Console.WriteLine("What is the customer's first name?");
            string first = Console.ReadLine();
            Console.WriteLine("What is the customer's last name?");
            string last = Console.ReadLine();
            Console.WriteLine("What is the customer's phone?");
            // extra validation on format, regular expression
            string phone = Console.ReadLine();
            Customer newCustomer = new Customer(social, first, last, phone, store);
            return newCustomer;
        }


        private static List<IProduct> ProductsSetup()
        {
            List<IProduct> productList = new List<IProduct>();
            Console.WriteLine("Enter 'x' to exit");
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
                double price = Double.TryParse(Console.ReadLine(), price);

                Console.WriteLine("Enter Product quantity");
                int quantity = Console.ReadLine();
                Product p = new Product(id, name, category, price, quantity);
                productList.Add(p);
            }
            return productList;



        }
    }
}
