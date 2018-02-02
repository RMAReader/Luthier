using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Luthier.Core
{
    [Serializable()]
    public class Serializer<T>
    {
        public const string StyleResearchNamespace = "a";
        public static XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();

        public static byte[] Serialize(T obj)
        {
            return ToXMLArray(obj);
        }

        public static T Deserialize(byte[] serializedObject)
        {
            return FromXMLArray(serializedObject);
        }

        public static byte[] ToXMLArray(T o)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                if (nameSpaces.Count == 0) nameSpaces.Add("", StyleResearchNamespace);
                xmlSerializer.Serialize(ms, o, nameSpaces);
                return ms.ToArray();
            }
        }

        public static T FromXMLArray(byte[] serializedObject)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(serializedObject))
            {
                return (T)xmlSerializer.Deserialize(ms);
            }
        }

        public static string ToXML(T o)
        {
            return ASCIIEncoding.ASCII.GetString(ToXMLArray(o));
        }
        public static T FromXML(string serializedObject)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(new System.IO.StringReader(serializedObject));
        }
    }

}
