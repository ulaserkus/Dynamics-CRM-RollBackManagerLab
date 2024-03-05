using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace RollBackManagerLab.CustomFlow.Extension
{
    public static class JsonExtension
    {
        public static string GetJsonValue(this string json, string key)
        {
            int keyIndex = json.IndexOf("\"" + key + "\":");
            if (keyIndex == -1)
            {
                return null;
            }

            int startIndex = keyIndex + key.Length + 3; // 3 = ":\""
            int endIndex = json.IndexOf("\"", startIndex);

            if (endIndex == -1)
            {
                // Değer bulunamadı
                return null;
            }

            return json.Substring(startIndex, endIndex - startIndex);
        }

        public static string SerializeToJson<TObject>(this TObject objects)
           where TObject : class
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TObject));
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, objects);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
        public static TObject DeserializeFromJson<TObject>(this string jsonString)
              where TObject : class
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TObject));
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (TObject)serializer.ReadObject(stream);
            }
        }
    }
}
