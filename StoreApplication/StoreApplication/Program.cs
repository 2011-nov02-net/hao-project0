using StoreApplication.Display;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace StoreApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // enter and exit

            string init = "";
            while (init != "x")
            {
                // to be implemented 
                Console.WriteLine("1. Enter store location to start:");
                string BranchID = Console.ReadLine();
                
                // all inputs should receive input validation  ex. CheckInput(string input)
                string path = "../../../SimplyWriteData.json";
                JsonFilePersist persist = new JsonFilePersist(path);
                Store store = persist.ReadStoreData();

                Console.WriteLine("1.Add a new customer\n  2.Process an order\n  3.Restock\n  4.Search in database\n  5.Display order details\n ");

                int choice = Console.Read();
                if (choice == 1)
                {
                    Console.WriteLine("What is the customer's SSN?");
                    // continue
                    string social = Console.ReadLine();
                    Console.WriteLine("What is the customer's first name?");
                    string first = Console.ReadLine();
                    Console.WriteLine("What is the customer's last name?");
                    string last = Console.ReadLine();
                    Console.WriteLine("What is the customer's phone?");
                    string phone = Console.ReadLine();

                    Console.WriteLine("What is the customer's default store?");
                    string branchID = Console.ReadLine();


                    // create an customer and add

                }
                else if (choice == 2)

                {
                }


                else if (choice == 3)


                { }

            // what to do, add a new cutomer, process an order, restock inventory


                // 1. add a new customer
                // ssn first last phone defaultlation

                // 2. process an order 
                // brandID used to create a store, other data used to create an customer
                // additional data to create and order
                // call customer.PlaceOrder

                //  3. restock
            }
        }
    }
}
