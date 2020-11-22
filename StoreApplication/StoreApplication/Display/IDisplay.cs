using StoreLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace StoreApplication.Display
{
    /// <summary>
    /// display interface that maps out multiple display functions
    /// </summary>
    public interface IDisplay
    {
        void DisplayOneOrder(COrder order);

        // for store and customer
        void DisplayAllOrders(List<COrder> orders);

        void DisplayAllStores(List<CStore> stores);

    }
}
