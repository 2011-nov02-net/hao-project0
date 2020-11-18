using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary.IDGenerator
{
    public static class OIDGen
    {
        private static int count = 888;

        public static string Gen()
        {
            count++;
            return "Order" + count;
        }

        public static string Get()
        {
            return "Order" + count;
        }
    }
}
