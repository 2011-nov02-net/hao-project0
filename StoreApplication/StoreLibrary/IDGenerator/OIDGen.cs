using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary.IDGenerator
{
    public static class OIDGen
    {
        

        public static string Gen()
        {
            string path = "../../../OID.txt";
            JsonFilePersist persist = new JsonFilePersist(path);
            string data = persist.ReadText();
            int number = int.Parse(data);
            number = number + 1;
            string id = number.ToString();
            persist.WriteText(id);
            string orderid = "order " + id;
            return orderid;
        }

        public static string Get()
        {
            string path = "../../../OID.txt";
            JsonFilePersist persist = new JsonFilePersist(path);
            string id = persist.ReadText();         
            string orderid = "Order " + id ; 
            return orderid;
        }
    }
}
