using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    /// <summary>
    /// store class, contains several behaviors
    /// add a customer, update a customer's order history, check and update inventory
    /// </summary>
    public class CStore
    {
        /// <summary>
        /// property store location to uniquely identify a store
        /// </summary>
        public string Storeloc { get; set; }

        /// <summary>
        /// property phone number of a store
        /// </summary>
        public string Storephone { get; set; }

        /// <summary>
        /// property to track products left at a store location
        /// key: productid, value: product object reference
        /// </summary>
        public Dictionary<string, CProduct> Inventory { get; set; } = new Dictionary<string, CProduct>();

        /// <summary>
        /// property to keep track of all customers of a store
        /// key: customerid, value: customer object reference 
        /// </summary>
        public Dictionary<string, CCustomer> CustomerDict { get; set; } = new Dictionary<string, CCustomer>();
    
        /// <summary>
        /// default constructor
        /// </summary>
        public CStore() { }
    
        /// <summary>
        /// parameterized constructor
        /// </summary>
        public CStore(string storeloc) {
            Storeloc = storeloc;
        }

        public CStore(string storeLoc, string storePhone)
        {
            Storeloc = storeLoc;
            Storephone = storePhone;
        }

        /// <summary>
        /// parameterized constructor to have a store start with inventory
        /// </summary>
        public CStore(string storeloc, string storePhone, List<CProduct> supply)
        {
            Storeloc = storeloc;
            Storephone = storePhone;
            foreach (var product in supply)
            {              
                Inventory[product.UniqueID] = product;
            }          
        }

        /// <summary>
        /// calculate total cost of an order
        /// </summary>
        public double CalculateTotalPrice(List<CProduct> productList)
        {
            double total = 0;
            foreach (var product in productList)
            {
                total = total + product.Price * product.Quantity;
            }
            return total;
        }

        /// <summary>
        /// store's behavior to restock
        /// add a product's quantity if it already exists, otherwise create a new pair
        /// </summary>
        public void AddProducts(List<CProduct> supply)
        {
            foreach (var product in supply)
            {
                CProduct temp;
                if(Inventory.TryGetValue(product.UniqueID, out temp))
                {
                    Inventory[product.UniqueID].Quantity += product.Quantity;        

                }
                else
                    Inventory[product.UniqueID] = product;
            }
        }

        /// <summary>
        /// store's behavior to add a customer
        /// add a customer if he is currently not a member
        /// </summary>
        public void AddCustomer(CCustomer customer)
        {
            // create new profile
            string customerid = customer.Customerid;

            CCustomer tempo; 
            if (CustomerDict.TryGetValue(customerid, out tempo))
            {
                // already exist, no need to add 
            }
            else
            {
                // not found, create a new customer profile
                CustomerDict[customerid] = customer;
            }          
        }

        /// <summary>
        /// store's behavior to update a customer's order history
        /// add a customer's profile if he is currently not a member
        /// </summary>
        public void UpdateInventoryAndCustomerOrder(COrder order)
        {
            // only successful order gets updated
            if (CheckInventory(order))
            {
                UpdateInventory(order);
                // need to change
                string customerid = order.Customer.Customerid;
                // update customer's list of order after it has been accepted
                CCustomer tempo;
                if (CustomerDict.TryGetValue(customerid, out tempo))
                {
                    CustomerDict[customerid].OrderHistory.Add(order);                    
                }
                else
                {
                    CustomerDict[customerid] = order.Customer;
                    CustomerDict[customerid].OrderHistory.Add(order);
                }            
            }   
        }

        /// <summary>
        /// store's behavior to update its inventory after a successful order
        /// </summary>
        public void UpdateInventory(COrder order)
        {
            // double checking just in case this method is called independently
            if (CheckInventory(order))
            {
                foreach (var purchasedProduct in order.ProductList)
                {
                    // update inventory
                    Inventory[purchasedProduct.UniqueID].Quantity -= purchasedProduct.Quantity;
                }
            }      
        }

        /// <summary>
        /// store's behavior to validate an order based on current product quantity
        /// </summary>
        public bool CheckInventory(COrder order)
        {
            foreach (var purchasedProduct in order.ProductList)
            {               
                string uniqueID = purchasedProduct.UniqueID;
                CProduct storage;
                if (Inventory.TryGetValue(uniqueID, out storage))
                {
                    // found but not enough 
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

        /// <summary>
        /// used to reset invetory
        /// </summary>
        public void CleanInventory()
        {
            Inventory = new Dictionary<string, CProduct>();
        }
     
    }
}
