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
            int menuUID = 0;

            if (Events.Depois(pVal) && pVal.MenuUID.Contains("Frm"))
            {
                string sfr_path = GetSRFPath(pVal);
                if (File.Exists(sfr_path))
                {
                    try
                    {
                        CriarForm(sfr_path);
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
            else if (Events.Depois(pVal) && int.TryParse(pVal.MenuUID, out menuUID))
            {
                if (Enum.IsDefined(typeof(EventosInternos), menuUID) && FormEvents._mappedInternalEventsToFormTypes.ContainsKey((EventosInternos)menuUID))
                {
                    EventosInternos eventoInterno = (EventosInternos)menuUID;

                    SAPbouiCOM.Form form = Global.SBOApplication.Forms.ActiveForm;
                    if (form != null && FormEvents._mappedInternalEventsToFormTypes[eventoInterno].ContainsKey(form.TypeEx))
                    {
                        var formObjType = Activator.CreateInstance(FormEvents._mappedInternalEventsToFormTypes[eventoInterno][form.TypeEx]);
                        ((SAPHelper.Form)formObjType)._OnAdicionarNovo(form);
                    }
                }
            }
        }

        private static string GetSRFPath(MenuEvent pVal)
        {
            return binFolder + "/" + pVal.MenuUID + ".srf";
        }

        private static void CriarForm(string sfr_path)
        {
            FormCreationParams creationPackage = Global.SBOApplication.CreateObject(BoCreatableObjectType.cot_FormCreationParams);

            var oXMLDoc = new XmlDocument();
            oXMLDoc.Load(sfr_path);
            creationPackage.XmlData = oXMLDoc.InnerXml;
            creationPackage.UniqueID = Guid.NewGuid().ToString("N");
            SAPbouiCOM.Form oForm = Global.SBOApplication.Forms.AddEx(creationPackage);

            oForm.Visible = true;
        }
    }
}