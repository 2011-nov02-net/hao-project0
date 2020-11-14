using System;
using System.Collections.Generic;

#nullable disable

namespace StoreDatamodel
{
    public partial class Orderr
    {
        public Orderr()
        {
            Orderproducts = new HashSet<Orderproduct>();
        }

        public string Orderid { get; set; }
        public string Storeloc { get; set; }
        public string Customerid { get; set; }
        public DateTime Orderedtime { get; set; }
        public double Totalcost { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Store StorelocNavigation { get; set; }
        public virtual ICollection<Orderproduct> Orderproducts { get; set; }
    }
}
