using SAPbouiCOM;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace SAPHelper
{
    public static class Menu
    {
        private static string binFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void CriarMenus(string xmlpath)
        {
            LoadBatches(xmlpath);
        }

        public static void RemoverMenus(string xmlpath)
        {
            LoadBatches(xmlpath);
        }

        private static void LoadBatches(string xmlpath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlpath);
            Global.SBOApplication.LoadBatchActions(xml.InnerXml);
        }

        public static void MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (Events.Depois(pVal) && pVal.MenuUID.Contains("Frm"))
            {
                string sfr_path = binFolder + "/" + pVal.MenuUID + ".srf";
                if (File.Exists(sfr_path))
                {
                    try
                    {
                        FormCreationParams creationPackage = Global.SBOApplication.CreateObject(BoCreatableObjectType.cot_FormCreationParams);

                        var oXMLDoc = new XmlDocument();
                        oXMLDoc.Load(sfr_path);
                        creationPackage.XmlData = oXMLDoc.InnerXml;

                        creationPackage.UniqueID = Guid.NewGuid().ToString("N");//pVal.MenuUID + DateTime.Now.Minute.ToString();

                        SAPbouiCOM.Form oForm = Global.SBOApplication.Forms.AddEx(creationPackage);

                        oForm.Visible = true;
                    }
                    catch (Exception e)
                    {
                        Dialogs.PopupError($"Erro ao abrir o formulário {pVal.MenuUID}.\nErro: " + e.Message);
                    }
                }
                else
                {
                    Dialogs.Info($"form {pVal.MenuUID} não foi encontrado na pasta raíz.");
                }
            }
        }
    }
}