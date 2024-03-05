using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace RollBackManagerLab.CustomFlow.Extension
{
    public static class CrmExtension
    {
        public static string GetAttributeValueAsString(this object value)
        {
            if (value == null)
            {
                return null;
            }
            else if (value.GetType().FullName.StartsWith("Microsoft.Xrm.Sdk"))
            {
                return SerializeMicrosoftXrmObject(value);
            }

            return value.ToString();
        }

        public static object GetAttributeValueFromString(this string value, string typeName)
        {
            if (value == null)
            {
                return null;
            }
            else if (typeName.StartsWith("Microsoft.Xrm.Sdk"))
            {
                return DeserializeMicrosoftXrmObject(value, Type.GetType(typeName));
            }
            else if (typeName.StartsWith("System.Guid"))
            {
                return Guid.Parse(value);
            }

            return Convert.ChangeType(value, Type.GetType(typeName));
        }

        private static string SerializeMicrosoftXrmObject(object value)
        {
            DataContractSerializer serializer = new DataContractSerializer(value.GetType());

            // Serialize the object
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, value);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        private static object DeserializeMicrosoftXrmObject(string serializedValue, Type objectType)
        {
            DataContractSerializer serializer = new DataContractSerializer(objectType);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(serializedValue)))
            {
                return serializer.ReadObject(stream);
            }
        }
    }
}
