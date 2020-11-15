﻿using StoreApplication.Display;
using StoreLibrary;
using StoreLibrary.Search;
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

            // all inputs have a validation method to process Ivalid inputs
            // to do
            string init = Console.ReadLine();
            while (init != "x")
            {
                // read from databse
                string path = "../../../SimplyWriteData.json";
                JsonFilePersist persist = new JsonFilePersist(path);
                CStore store = persist.ReadStoreData();

                Console.WriteLine("Choose one of the following operations:\n 1.Add a new customer\n  2.Process an order\n  3.Restock\n  4.Search in database\n  5.Display order detail\n ");
                // extra validation

                int choice = Console.Read();
                if (choice == 1)
                {
                    CCustomer newCustomer = CustomerSetup(store);                                        
                    store.AddCustomer(newCustomer);
                }
                else if (choice == 2)
                {
                    Console.WriteLine("What is the customer's first name?");
                    string firstname = Console.ReadLine();
                    Console.WriteLine("What is the customer's last name?");
                    string lastname = Console.ReadLine();

                    ISearch ss = new SimpleSearch();
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
                }

                else if (choice == 3)
                {
                    List<CProduct> supply = ProductsSetup();                                      
                    store.AddProducts(supply);
                }

                else if (choice == 4)
                {
                    // refactor this portion into a method
                    Console.WriteLine("Enter Customer's first name");
                    string firstname = Console.ReadLine();
                    Console.WriteLine("Enter Customer's last name");
                    string lastname = Console.ReadLine();

                    SimpleSearch ss = new SimpleSearch();
                    // empty string if not found
                    string customerid;
                    bool found = ss.SearchByName(store, firstname, lastname, out customerid);
                    if (found)
                    {
                        Console.WriteLine("customer profile does not exist");
                    }
                    else { Console.WriteLine("customer profile found"); }

                }
                else if (choice == 5)
                {
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

                }

            }
        }


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


        private static List<CProduct> ProductsSetup()
        {
            List<CProduct> productList = new List<CProduct>();
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
                string priceStr = Console.ReadLine();
                double price;
                Double.TryParse(priceStr,out price);

                Console.WriteLine("Enter Product quantity");
                string quantityStr = Console.ReadLine();
                int quantity;
                int.TryParse(quantityStr, out quantity);
                CProduct p = new CProduct(id, name, category, price, quantity);
                productList.Add(p);
            }
            return productList;



        }
    }
}
