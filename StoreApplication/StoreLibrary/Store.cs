using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    public class Store
    {
        // use the uniqueID to keep track of the same type of items
        // convert Product type into string
        private Dictionary<string, int> Inventory = new Dictionary<string, int>();

        // constructor
        public Store(List<IProduct> supply)
        {
            foreach (var product in supply)
            {
                Inventory[product.UniqueID] = product.Quantity;
            }
            
        }

        //
        private List<Order> orderList = new List<Order>();

        public void UpdateOrder(Order order)
        {
            // successful order gets stored
            if(CheckUpdateInventory(order))
                orderList.Add(order);
            
        }

        // decrease inventory
        public bool CheckUpdateInventory(Order order)
        {
            // check type
            // check quantity
            // update quantity
            
            int counter = 0;
            foreach (var purchasedProduct in order.ProductList)
            {
                // because of reference types, same objects may not be considered the same
                // try string literals
                string uniqueID = purchasedProduct.UniqueID;
                int storage;
                // find the product in the store inventory
                if (Inventory.TryGetValue(uniqueID, out storage))
                {
                    // found
                    // but not enough 
                    if (storage < purchasedProduct.Quantity)
                    {
                        return false;
                    }
                    // enough
                    else
                    {
                        // one product's quantity has qualified
                        counter++;
                    }
                }
                else 
                {
                    // not found
                    return false;
                }
            }

            // all requirement met
            if (counter == order.ProductList.Count)
            {
                foreach (var purchasedProduct in order.ProductList)
                {
                    // update inventory
                    Inventory[purchasedProduct.UniqueID] -= purchasedProduct.Quantity;
                }
            }
            return true;
        }
    }
}
