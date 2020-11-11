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
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.Formatting = Formatting.Indented;      
            using (StreamWriter sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, data, typeof(Store));
            }   
            /*
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(path,json);
            */         
        }
        public Store ReadStoreData()
        {
            Store data;
            try
            {
                
                string json = File.ReadAllText(path);
                /*
                // object cycle
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
                */
                data = JsonConvert.DeserializeObject<Store>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore,
                });
            }
            catch (FileNotFoundException e)
            {
                return new Store();
            }
            return data;
        }
    }
}
