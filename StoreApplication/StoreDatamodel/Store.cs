using System;
using System.Collections.Generic;

#nullable disable

namespace StoreDatamodel
{
    public partial class Store
    {
        public Store()
        {
            Inventories = new HashSet<Inventory>();
            Orderrs = new HashSet<Orderr>();
            Storecustomers = new HashSet<Storecustomer>();
        }

        public string Storeloc { get; set; }
        public string Storephone { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<Orderr> Orderrs { get; set; }
        public virtual ICollection<Storecustomer> Storecustomers { get; set; }
    }
}
