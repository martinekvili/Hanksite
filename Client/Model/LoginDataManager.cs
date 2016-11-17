using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Client.Model
{
    public class LoginDataManager
    {
        public LoginData LoadLastLogin()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("lastlogin.xml");
            string xml = xmlDocument.InnerXml;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LoginData));
            using (var stringReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    return (LoginData)xmlSerializer.Deserialize(xmlReader);
                }
            }
        }

        public void SaveLastLogin(LoginData loginData)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LoginData));
            string xml;

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter))
                {
                    xmlSerializer.Serialize(xmlWriter, loginData);
                    xml = stringWriter.ToString();
                }
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            xmlDocument.Save("lastlogin.xml");
        }
    }
}
