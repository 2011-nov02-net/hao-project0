using StoreApplication.Display;
using StoreLibrary;
using System;
using System.Collections.Generic;

namespace StoreApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            IDisplay dis = new SimpleDisplay();

            List<IProduct> supply = new List<IProduct>
            { new Product("111","apple","Produce",1.0,1),
              new Product("222","orange","Produce",0.88,10)};

            Store store = new Store("Phoenix101", supply);
            Customer customer = new Customer("123123121", "John", "Smith", "6021111111", store);
            Order order = new Order(store, customer, DateTime.Today, supply);

            dis.DisplayOneOrder(order);
        }
    }
}
