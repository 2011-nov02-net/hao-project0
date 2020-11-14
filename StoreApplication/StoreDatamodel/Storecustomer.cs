using System;
using System.Collections.Generic;

#nullable disable

namespace StoreDatamodel
{
    public partial class Storecustomer
    {
        public int Relationid { get; set; }
        public string Storeloc { get; set; }
        public string Customerid { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Store StorelocNavigation { get; set; }
    }
}
