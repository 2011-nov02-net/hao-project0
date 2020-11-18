using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary.IDGenerator
{
    public static class CIDGen
    {
        private static int count = 888;

        public static string Gen()
        {
            count++;
            return "Customer" + count;
        }

        public static string Get()
        {
            return "Customer" + count;
        }
    }
}
