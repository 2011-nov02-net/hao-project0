using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;



namespace StoreLibrary
{
    public class JsonFilePersist
    {
        private readonly string path;
        public JsonFilePersist(string cpath)
        {
            path = cpath;

        }


        
        public void WriteStoreData(Store data)
        {
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(path,json);
        }

        // multiple ways to read
        public Store ReadStoreData()
        {
            Store data;
            try
            {
                string json = File.ReadAllText(path);
                // object cycle

                data = JsonConvert.DeserializeObject<Store>(json);
            }
            catch (FileNotFoundException e)
            {
                return new Store();
            }
            return data;
        }
    }
}
