using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;

namespace xmlTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string xml;

            using (WebClient client = new WebClient())
            {
                xml = client.DownloadString("https://www.dropbox.com/s/3pfym0u1kznsqr0/versions.xml?dl=1");
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNodeList nl = doc.SelectNodes("versions");
            XmlNode root = nl[0];

            foreach (XmlNode xnode in root.ChildNodes)
            {
                Console.WriteLine(xnode.ChildNodes.Item(0).InnerText);
                Console.WriteLine(xnode.ChildNodes.Item(1).InnerText);
            }

            Console.Read();
        }
    }
}
