using SAPbouiCOM;
using System;
using System.Xml;

namespace SAPHelper
{
    public abstract class Form
    {

        public abstract string FormType { get; }

        #region :: Form Data Add

        public virtual void OnBeforeFormDataAdd(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterFormDataAdd(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        #endregion


        #region :: Form Data Update

        public virtual void OnBeforeFormDataUpdate(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterFormDataUpdate(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        #endregion


        #region :: Form Data Delete

        public virtual void OnBeforeFormDataDelete(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterFormDataDelete(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        #endregion


        #region :: Form Data Load

        public virtual void OnBeforeFormDataLoad(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterFormDataLoad(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        #endregion


        #region :: Form Load

        public virtual void OnBeforeFormLoad(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterFormLoad(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        #endregion


        #region :: Form Draw

        public virtual void OnBeforeFormDraw(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterFormDraw(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        #endregion


        #region :: Form Close

        public virtual void OnBeforeFormClose(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterFormClose(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        #endregion


        #region :: Form Visible

        public virtual void OnBeforeFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        #endregion


        #region :: UDO Form Open

        public virtual void OnBeforeUDOFormOpen(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterUDOFormOpen(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        #endregion


        #region :: Click


        public virtual void OnBeforeClick(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterClick(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        #endregion


        #region :: Combo Select


        public virtual void OnBeforeComboSelect(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterComboSelect(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        #endregion


        #region :: Choose From List


        public virtual void OnBeforeChooseFromList(SAPbouiCOM.Form form, ChooseFromListEvent chooseEvent, ChooseFromList choose, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterChooseFromList(SAPbouiCOM.Form form, ChooseFromListEvent chooseEvent, ChooseFromList choose, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        #endregion


        #region :: Item Pressed

        public virtual void OnBeforeItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        #endregion


        #region :: Eventos Internos

        public virtual void _OnAdicionarNovo(SAPbouiCOM.Form form)
        {
        }

        public virtual void _OnPesquisar(SAPbouiCOM.Form form)
        {
        }

        #endregion


        #region :: Utils

        protected SAPbouiCOM.Form GetForm(string formUID)
        {
            return Global.SBOApplication.Forms.Item(formUID);
        }

        protected DBDataSource GetDBDatasource(SAPbouiCOM.Form form, string dbdts_name)
        {
            return form.DataSources.DBDataSources.Item(dbdts_name);
        }

        protected DBDataSource GetDBDatasource(string formUID, string dbdts_name)
        {
            SAPbouiCOM.Form form = GetForm(formUID);
            return form.DataSources.DBDataSources.Item(dbdts_name);
        }

        protected void Copy(DBDataSource dbdts_from, ref DBDataSource dbdts_to)
        {
            dbdts_to.Clear();
            for (int i = 0; i < dbdts_from.Size; i++)
            {
                dbdts_to.InsertRecord(i);
                for (int j = 0; j < dbdts_from.Fields.Count; j++)
                {
                    string value = dbdts_from.GetValue(j, i);
                    dbdts_to.SetValue(j, i, value);
                }
            }
        }

        protected string GetNextPrimaryKey(string tabela_com_arroba, string campo)
        {
            var rs = Helpers.DoQuery(
                $@"SELECT 
	                    CASE 
		                    WHEN (SELECT COUNT(*) FROM [{tabela_com_arroba}]) = 0
			                    THEN 1
		                    ELSE
			                    (SELECT (MAX({campo})+1) FROM [{tabela_com_arroba}])
	                    END as ultimo"
            );

            return rs.Fields.Item(0).Value.ToString();
        }

        protected string GetNextCode(string tabela_com_arroba)
        {
            string next_code = GetNextPrimaryKey(tabela_com_arroba, "Code");

            return next_code.PadLeft(4, '0');
        }

        protected void PopularComboBox(SAPbouiCOM.Form form, string comboUID, string sql)
        {
            ComboBox comboBox = ((ComboBox)form.Items.Item(comboUID).Specific);
            PopularComboBox(comboBox, sql);
        }

        protected void PopularComboBox(ComboBox comboBox, string sql)
        {
            RemoverTodosValoresValidados(comboBox);
            AcrescentarValoresValidados(comboBox, sql);
        }

        protected void AcrescentarValoresValidados(ComboBox comboBox, string sql)
        {
            var rs = Helpers.DoQuery(sql);
            while (!rs.EoF)
            {
                comboBox.ValidValues.Add(rs.Fields.Item(0).Value.ToString(), rs.Fields.Item(1).Value.ToString());

                rs.MoveNext();
            }
        }

        protected void RemoverTodosValoresValidados(ComboBox comboBox)
        {
            var count = comboBox.ValidValues.Count;
            for (int i = 0; i < count; i++)
            {
                comboBox.ValidValues.Remove(0, BoSearchKey.psk_Index);
            }
        }

        protected void ValidarCamposObrigatorios(DBDataSource dbdts)
        {
            var props = GetType().GetFields();
            foreach (var prop in props)
            {
                if (prop.FieldType == typeof(ItemFormObrigatorio))
                {
                    var propriedade = (ItemFormObrigatorio)prop.GetValue(this);
                    var valor = dbdts.GetValue(propriedade.Datasource, 0).Trim();

                    if (string.IsNullOrEmpty(valor))
                    {
                        throw new FormValidationException(propriedade.Mensagem, propriedade.ItemUID);
                    }
                }
            }
        }

        public static void CriarForm(string sfr_path)
        {
            SAPbouiCOM.Form oForm = CreationPackage(sfr_path);

            oForm.Visible = true;
        }

        public static void CriarFormFilho(string sfr_path, string fatherFormUID, Form formFilho)
        {
            SAPbouiCOM.Form oForm = CreationPackage(sfr_path);

            var fatherFormUIDField = formFilho.GetType().GetField("_fatherFormUID");
            if (fatherFormUIDField != null)
            {
                fatherFormUIDField.SetValue(formFilho, fatherFormUID);

                oForm.Visible = true;
            }
            else
            {
                Dialogs.PopupError("Erro interno. Erro de desenvolvimento.\nField estático '_fatherFormUID' não encontrado na classe filha");
            }
        }

        private static SAPbouiCOM.Form CreationPackage(string sfr_path)
        {
            FormCreationParams creationPackage = Global.SBOApplication.CreateObject(BoCreatableObjectType.cot_FormCreationParams);

            var oXMLDoc = new XmlDocument();
            oXMLDoc.Load(sfr_path);
            creationPackage.XmlData = oXMLDoc.InnerXml;
            creationPackage.UniqueID = Guid.NewGuid().ToString("N");
            SAPbouiCOM.Form oForm = Global.SBOApplication.Forms.AddEx(creationPackage);
            return oForm;
        }

        #endregion
    }
}
