using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary.IDGenerator
{
    public static class PIDGen
    {
        private static int count = 1000;

        public static string Gen()
        {
            count++;
            return "Product" + count;
        }

        public static string Get()
        {
            return "Product" + count;
        }
    }
}
