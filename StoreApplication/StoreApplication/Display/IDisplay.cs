using StoreLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace StoreApplication.Display
{
    public interface IDisplay
    {
        void DisplayOneOrder(Order order);

        // for store and customer
        void DisplayAllOrder(List<Order> orders);
    }
}
