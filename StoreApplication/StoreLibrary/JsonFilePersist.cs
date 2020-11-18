
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;

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
        /// simplified only to write a number to a text file
        /// </summary>
        public void WriteText(string data)
        {           
            File.WriteAllText(path,data);           
        }

        /// <summary>
        /// simplified only to read a number from a text file
        /// </summary>
        public string ReadText()
        {
            string data;
            try
            {         
                data = File.ReadAllText(path);               
            }
            catch (FileNotFoundException e)
            {
                data = "777";
            }
            return data;
        }


        /// <summary>
        /// original behavior to serialize and write data
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
        /// original behavior to read data and deserialize
        /// have jsonignore on model order class to avoid object cycle
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
