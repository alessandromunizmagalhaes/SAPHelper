using System.Xml;

namespace SAPHelper
{
    public static class Menu
    {
        public static SAPbouiCOM.Application SBOApplication;

        public static void CriarMenus(string xmlpath)
        {
            LoadBatches(xmlpath);
        }

        public static void RemoverMenus(string xmlpath)
        {
            LoadBatches(xmlpath);
        }

        public static void LoadBatches(string xmlpath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlpath);
            SBOApplication.LoadBatchActions(xml.InnerXml);
        }

        public static void RecebeSBOApplication(SAPbouiCOM.Application application)
        {
            SBOApplication = application;
        }
    }
}