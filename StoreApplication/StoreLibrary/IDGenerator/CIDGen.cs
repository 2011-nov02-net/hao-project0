using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary.IDGenerator
{
    public static class CIDGen
    {
        public static string Gen()
        {
            string path = "../../../CID.txt";
            JsonFilePersist persist = new JsonFilePersist(path);
            string data = persist.ReadText();
            int number = int.Parse(data);
            number = number + 1;
            string id = number.ToString();
            persist.WriteText(id);
            string customerid = "customer " + id;
            return customerid;
        }

        public static string Get()
        {
            string path = "../../../CID.txt";
            JsonFilePersist persist = new JsonFilePersist(path);
            string id = persist.ReadText();
            string customerid = "customer " + id;
            return customerid;
        }
    }
}
