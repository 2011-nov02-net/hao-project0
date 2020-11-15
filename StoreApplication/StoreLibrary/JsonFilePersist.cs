using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;



namespace StoreLibrary
{
    /// <summary>
    /// persistent data handling
    /// </summary>
    public class JsonFilePersist
    {
        private readonly string path;
        /// <summary>
        /// default constructor
        /// </summary>
        public JsonFilePersist(string cpath)
        {
            path = cpath;
        }
      
        /// <summary>
        /// behavior to serialize and write data
        /// </summary>
        public void WriteStoreData(CStore data)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.Formatting = Formatting.Indented;      
            using (StreamWriter sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, data, typeof(CStore));
            }   
            /*
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(path,json);
            */         
        }

        /// <summary>
        /// behavior to read data and deserialize
        /// have jsonignore on console order class to avoid object cycle
        /// </summary>
        public CStore ReadStoreData()
        {
            CStore data;
            try
            {
                
                string json = File.ReadAllText(path);
                /*
                // object cycle
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
                */
                data = JsonConvert.DeserializeObject<CStore>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore,
                });
            }
            catch (FileNotFoundException e)
            {
                return new CStore();
            }
            return data;
        }
    }
}
