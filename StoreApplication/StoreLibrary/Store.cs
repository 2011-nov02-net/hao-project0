using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    public class Store
    {     
        // unique
        public string BranchID { get; set; }
        public Dictionary<string, IProduct> Inventory { get; set; }

        // use a unique social to keep track of customers
        public Dictionary<string, Customer> CustomerDict { get; set; }

        // multiple constructors      
        public Store() { }
        public Store(string branchID) {
            BranchID = branchID;
            Inventory = new Dictionary<string, IProduct>();
            CustomerDict = new Dictionary<string, Customer>();
        }
        public Store(string branchID, List<IProduct> supply)
        {
            BranchID = branchID;
            Inventory = new Dictionary<string, IProduct>();
            CustomerDict = new Dictionary<string, Customer>();
            foreach (var product in supply)
            {              
                Inventory[product.UniqueID] = product;
            }          
        }

        public void AddProducts(List<IProduct> supply)
        {
            foreach (var product in supply)
            {
                IProduct temp;
                if(Inventory.TryGetValue(product.UniqueID, out temp))
                {
                    Inventory[product.UniqueID].Quantity += product.Quantity;        

                }
                else
                    Inventory[product.UniqueID] = product;
            }
        }

        public void AddCustomer(Customer customer)
        {
            // create new profile
            string social = customer.Social;

            Customer tempo; 
            if (CustomerDict.TryGetValue(social, out tempo))
            {
                // already exist, no need to add 
            }
            else
            {
                // not found, create a new customer profile
                CustomerDict[social] = customer;
            }          
        }


        public void UpdateCustomerOrder(Order order)
        {
            // only successful order gets updated
            if (CheckUpdateInventory(order))
            {
                // need to change
                string social = order.Customer.Social;
                // update customer's list of order after it has been accepted
                Customer tempo;
                if (CustomerDict.TryGetValue(social, out tempo))
                {
                    CustomerDict[social].OrderHistory.Add(order);                    
                }
                else
                {
                    CustomerDict[social] =  order.Customer;
                    CustomerDict[social].OrderHistory.Add(order);
                }
                
            }
            
        }

        // check and decrease inventory
        public bool CheckUpdateInventory(Order order)
        {
            // all requirement met
            if (CheckInventory(order))
            {
                foreach (var purchasedProduct in order.ProductList)
                {
                    // update inventory
                    Inventory[purchasedProduct.UniqueID].Quantity -= purchasedProduct.Quantity;
                }
            }
            return true;
        }

        public bool CheckInventory(Order order)
        {
            foreach (var purchasedProduct in order.ProductList)
            {
                // because of reference types, same objects may not be considered the same
                // try string literals
                string uniqueID = purchasedProduct.UniqueID;
                IProduct storage;
                // find the product in the store inventory
                if (Inventory.TryGetValue(uniqueID, out storage))
                {
                    // found
                    // but not enough 
                    if (storage.Quantity < purchasedProduct.Quantity)
                    {
                        return false;
                    }
                    // enough
                    else
                    {
                        // one product's quantity has qualified
                      
                    }
                }
                else
                {
                    // not found
                    return false;
                }
            }
            return true;

        }
    }
}
