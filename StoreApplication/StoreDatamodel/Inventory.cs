using System;
using System.Collections.Generic;

#nullable disable

namespace StoreDatamodel
{
    public partial class Inventory
    {
        public int Supplyid { get; set; }
        public string Storeloc { get; set; }
        public string Productid { get; set; }
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual Store StorelocNavigation { get; set; }
    }
}
