using System.Xml.Serialization;

namespace ConfigParsers.Common
{
    public static class ObjectDeserializer
    {
        public static T DeserializeToObject<T>(string filepath) where T : class
        {
            var ser = new XmlSerializer(typeof(T));

            using var sr = new StreamReader(filepath);
            var data = ser.Deserialize(sr);
            if (data == null) throw new Exception($"Could not Deserialize {filepath} to {typeof(T)}");
            return (T)data;
        }

    }
}
