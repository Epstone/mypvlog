using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace PVLog.Utility
{
    public class MyXmlSerializer<T>
    {
        XmlSerializer xmlSer = new XmlSerializer(typeof(T));

        public string SerializeObject(object obj)
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);


            xmlSer.Serialize(writer, obj);
            return builder.ToString();
        }

        // Objekt deserialisieren 
        public T DeserializeObject(string xml)
        {
            try
            {
                StringReader rdr = new StringReader(xml);
                return (T)xmlSer.Deserialize(rdr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }
    }
}