using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace SAPHelper
{
    public static class ConfigXML
    {
        private static string binFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string xmlPath = binFolder + "\\config.xml";

        public static bool JaCriouEstrutura
        {
            get
            {
                return EstruturaJaFoiCriada();
            }
            internal set
            {
                SetarEstruturaComoCriada();
            }
        }

        private static void SetarEstruturaComoCriada()
        {
            XmlDocument xml = new XmlDocument();
            if (File.Exists(xmlPath))
            {
                xml.Load(xmlPath);
                xml.SelectSingleNode("/config/jaCriouEstrutura").InnerText = "1";
                xml.Save(xmlPath);
            }
        }

        private static bool EstruturaJaFoiCriada()
        {
            XmlDocument xml = new XmlDocument();
            if (File.Exists(xmlPath))
            {
                xml.Load(xmlPath);
                return xml.SelectSingleNode("/config/jaCriouEstrutura").InnerText == "1";
            }
            else
            {
                throw new ArgumentException("O arquivo config.xml não foi encontrado");
            }
        }
    }
}
