using System;
using System.Collections.Generic;

#nullable disable

namespace StoreDatamodel
{
    public partial class Orderproduct
    {
        public int Processid { get; set; }
        public string Orderid { get; set; }
        public string Productid { get; set; }
        public int Quantity { get; set; }

        public virtual Orderr Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
