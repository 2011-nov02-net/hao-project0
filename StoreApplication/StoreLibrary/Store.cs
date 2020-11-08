using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    public class Store
    {
        // use a uniqueID to keep track of the same type of items
        // convert Product type into string
        public string BranchID { get; set; }
        public Dictionary<string, int> Inventory { get; set; }

        // use a unique social to keep track of customer's profile
        public Dictionary<string, List<Order>> CustomerDict { get; set; }

        // constructor      
        public Store(string branchID) {
            BranchID = branchID;
            Inventory = new Dictionary<string, int>();
            CustomerDict = new Dictionary<string, List<Order>>();
        }
        public Store(string branchID, List<IProduct> supply)
        {
            BranchID = branchID;
            Inventory = new Dictionary<string, int>();
            CustomerDict = new Dictionary<string, List<Order>>();
            foreach (var product in supply)
            {              
                Inventory[product.UniqueID] = product.Quantity;
            }          
        }

        public void AddCustomer(Customer customer)
        {
            // create new profile
            string social = customer.Social;

            List<Order> orderHistory; 
            if (CustomerDict.TryGetValue(social, out orderHistory))
            {
                // already exist
            }
            else
            {
                // not found, orderHistory set to default which is empty
                CustomerDict[social] = customer.OrderHistory;
            }          
        }


        public void UpdateStoreOrder(Order order)
        {
            // successful order gets stored
            if (CheckUpdateInventory(order))
            {
                string social = order.Customer.Social;
                // update customer's list of order after it has been accepted
                List<Order> tempo;
                if (CustomerDict.TryGetValue(social, out tempo))
                {
                    CustomerDict[social].Add(order);
                }
                else
                { 
                    CustomerDict[social] = new List<Order>();
                    CustomerDict[social].Add(order);
                }
                
            }
            
        }

        // check and decrease inventory
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
