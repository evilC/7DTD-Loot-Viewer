using System.Xml.Serialization;

namespace ConfigParsers.Common
{
    public static class ObjectDeserializer
    {
        public static T DeserializeToObject<T>(string filepath) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StreamReader sr = new StreamReader(filepath))
            {
                return (T)ser.Deserialize(sr);
            }
        }

    }
}
