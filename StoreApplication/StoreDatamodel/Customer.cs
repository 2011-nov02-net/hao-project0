using System;
using System.Collections.Generic;

#nullable disable

namespace StoreDatamodel
{
    public partial class Customer
    {
        public Customer()
        {
            Orderrs = new HashSet<Orderr>();
            Storecustomers = new HashSet<Storecustomer>();
        }

        public string Customerid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phonenumber { get; set; }

        public virtual ICollection<Orderr> Orderrs { get; set; }
        public virtual ICollection<Storecustomer> Storecustomers { get; set; }
    }
}
