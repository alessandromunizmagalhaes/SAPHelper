using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Reflection;
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



        #region :: Double Click


        public virtual void OnBeforeDoubleClick(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterDoubleClick(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        #endregion


        #region :: Validate


        public virtual void OnBeforeValidate(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterValidate(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
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


        #region :: Matrix Linked Pressed

        public virtual void OnBeforeMatrixLinkPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        public virtual void OnAfterMatrixLinkPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
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

        protected void DesabilitarMenuAdicionarNovo(SAPbouiCOM.Form form)
        {
            form.EnableMenu(((int)EventosInternos.AdicionarNovo).ToString(), false);
        }

        protected void DesabilitarMenuPesquisar(SAPbouiCOM.Form form)
        {
            form.EnableMenu(((int)EventosInternos.Pesquisar).ToString(), false);
        }

        #endregion


        #region :: Utils

        public static void CriarForm(string sfr_path, CriarFormParams criarFormParams = null)
        {
            SAPbouiCOM.Form oForm = CreationPackage(sfr_path);

            AplicarFormParams(oForm, criarFormParams);
        }

        public static void CriarFormFilho(string sfr_path, string fatherFormUID, Form formFilho, CriarFormParams criarFormParams = null)
        {
            SAPbouiCOM.Form oForm = CreationPackage(sfr_path);

            // usando esses bindingflags porque se o formFilho for derivado/herdade de uma classe pai
            // e nessa classe pai estiver o _fatherFormUID, sem esses bindingflags ele não enxerga
            // porque não é uma propriedade do filho e sim do pai
            // colocando esses bindingflags resolve
            // https://stackoverflow.com/questions/1325280/c-sharp-reflection-base-class-static-fields-in-derived-type
            var fatherFormUIDField = formFilho.GetType().GetField("_fatherFormUID", BindingFlags.Static
             | BindingFlags.FlattenHierarchy
             | BindingFlags.Public);
            if (fatherFormUIDField != null)
            {
                fatherFormUIDField.SetValue(formFilho, fatherFormUID);

                AplicarFormParams(oForm, criarFormParams);
            }
            else
            {
                Dialogs.PopupError("Erro interno. Erro de desenvolvimento.\nField estático '_fatherFormUID' não encontrado na classe filha");
            }
        }

        private static void AplicarFormParams(SAPbouiCOM.Form oForm, CriarFormParams criarFormParams)
        {
            bool criarParamsNotNull = criarFormParams != null;
            if (criarParamsNotNull)
            {
                oForm.Mode = criarFormParams.Mode;
            }

            oForm.Visible = true;

            if (criarParamsNotNull && criarFormParams.FindParams != null)
            {
                try
                {
                    oForm.Freeze(true);
                    var findParams = criarFormParams.FindParams;
                    oForm.Items.Item(findParams.chavePrimariaUID).Specific.Value = findParams.chavePrimariaValor;
                    oForm.Items.Item("1").Click();
                }
                catch (Exception e)
                {
                    Dialogs.PopupError($"Erro de desenvolvimento.\nErro ao tentar encontrato o registro.\nErro: " + e.Message);
                }
                finally
                {
                    oForm.Freeze(false);
                }
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

        /*
        protected SAPbouiCOM.Form GetForm(string formUID)
        {
            return Global.SBOApplication.Forms.Item(formUID);
        }
        */

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formUID"></param>
        /// <returns>Se o Form não existir, retorna NULL</returns>
        protected SAPbouiCOM.Form GetFormIfExists(string formUID)
        {
            SAPbouiCOM.Form form = null;
            for (int i = 0; i < Global.SBOApplication.Forms.Count; i++)
            {
                var currentForm = Global.SBOApplication.Forms.Item(i);
                if (currentForm.UniqueID == formUID)
                {
                    form = currentForm;
                    break;
                }
            }
            return form;
        }

        protected DBDataSource GetDBDatasource(SAPbouiCOM.Form form, string dbdts_name)
        {
            return form.DataSources.DBDataSources.Item(dbdts_name);
        }

        protected DBDataSource GetDBDatasource(string formUID, string dbdts_name)
        {
            using (var formCOM = new FormCOM(formUID))
            {
                SAPbouiCOM.Form form = formCOM.Form;
                return form.DataSources.DBDataSources.Item(dbdts_name);
            }
        }

        protected DataTable GetDatatable(SAPbouiCOM.Form form, string datatable_name)
        {
            return form.DataSources.DataTables.Item(datatable_name);
        }

        protected DataTable GetDatatable(string formUID, string datatable_name)
        {
            using (var formCOM = new FormCOM(formUID))
            {
                SAPbouiCOM.Form form = formCOM.Form;
                return form.DataSources.DataTables.Item(datatable_name);
            }
        }

        protected Matrix GetMatrix(string formUID, string matrixUID)
        {
            using (var formCOM = new FormCOM(formUID))
            {
                SAPbouiCOM.Form form = formCOM.Form;
                return GetMatrix(form, matrixUID);
            }
        }

        protected Matrix GetMatrix(SAPbouiCOM.Form form, string matrixUID)
        {
            return ((Matrix)form.Items.Item(matrixUID).Specific);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbdts_from"></param>
        /// <param name="dbdts_to"></param>
        /// <param name="fieldsNotIn">campos separados por virgula para já imbutir no notin, exemplo: ",'campo1','campo2','campo3'"</param>
        protected void CopyIfFieldsMatch(DBDataSource dbdts_from, ref DBDataSource dbdts_to, string fieldsNotIn = "")
        {
            using (var recordset = new RecordSet())
            {
                var rs = recordset.DoQuery(
                $@"SELECT 
	                tb1.COLUMN_NAME
                FROM INFORMATION_SCHEMA.COLUMNS tb1
                INNER JOIN INFORMATION_SCHEMA.COLUMNS tb2 ON (tb2.COLUMN_NAME = tb1.COLUMN_NAME)
                WHERE 1 = 1
                    AND tb1.TABLE_NAME = '{dbdts_from.TableName}' AND tb2.TABLE_NAME = '{dbdts_to.TableName}'
                    AND tb1.COLUMN_NAME NOT IN 
                        ('Code','Name','Canceled','CreateDate','CreateTime',
                        'DataSource','DocEntry','LogInst','Object','Transfered',
                        'UpdateDate','UpdateTime','UserSign' {fieldsNotIn})"
                        );

                dbdts_to.Clear();
                for (int i = 0; i < dbdts_from.Size; i++)
                {
                    dbdts_to.InsertRecord(i);
                    while (!rs.EoF)
                    {
                        var campo = rs.Fields.Item("COLUMN_NAME").Value;
                        string value = dbdts_from.GetValue(campo, i);
                        dbdts_to.SetValue(campo, i, value);

                        rs.MoveNext();
                    }

                    rs.MoveFirst();
                }
            }
        }

        protected string GetNextPrimaryKey(string tabela_com_arroba, string campo)
        {
            using (var recordset = new RecordSet())
            {
                var rs = recordset.DoQuery(
                $@"SELECT 
	                    CASE 
		                    WHEN (SELECT COUNT(*) FROM [{tabela_com_arroba}]) = 0
			                    THEN 1
		                    ELSE
			                    (SELECT (MAX(CONVERT(INT,{campo}))+1) FROM [{tabela_com_arroba}])
	                    END as ultimo"
                );

                return rs.Fields.Item(0).Value.ToString();
            }
        }

        protected string GetNextCode(string tabela_com_arroba)
        {
            string next_code = GetNextPrimaryKey(tabela_com_arroba, "Code");

            return next_code.PadLeft(4, '0');
        }

        public bool CamposFormEstaoPreenchidos(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            try
            {
                ValidarCamposObrigatorios(dbdts, this);
            }
            catch (FormValidationException e)
            {
                Dialogs.MessageBox(e.Message);
                if (!String.IsNullOrEmpty(e.AbaUID))
                {
                    form.Items.Item(e.AbaUID).Click();
                }
                form.Items.Item(e.Campo).Click();
                return false;
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro interno. Erro ao tentar atribuir valores do formulário.\nErro: " + e.Message);
                return false;
            }
            return true;
        }

        public bool CamposMatrizEstaoValidos(SAPbouiCOM.Form form, DBDataSource dbdts, MatrizForm _matriz)
        {
            try
            {
                ValidarCamposObrigatorios(dbdts, _matriz);
            }
            catch (FormValidationException e)
            {
                Dialogs.MessageBox(e.Message);
                if (!String.IsNullOrEmpty(e.AbaUID))
                {
                    form.Items.Item(e.AbaUID).Click();
                }
                var mtx = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);
                if (mtx.RowCount > 0)
                {
                    mtx.Columns.Item(e.Campo).Cells.Item(e.DatasourceRow + 1).Click();
                }

                return false;
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro interno. Erro ao tentar atribuir valores da matriz ao formulário.\nErro: " + e.Message);
                return false;
            }
            return true;
        }

        private void ValidarCamposObrigatorios(DBDataSource dbdts, object objWithProperties)
        {
            var props = objWithProperties.GetType().GetFields();
            foreach (var prop in props)
            {
                if (typeof(IItemFormObrigatorio).IsAssignableFrom(prop.FieldType))
                {
                    ValorObrigatorio(dbdts, objWithProperties, prop);
                }
                else if (typeof(IItemFormObrigatorioUnico).IsAssignableFrom(prop.FieldType))
                {
                    ValorObrigatorioEUnico(dbdts, objWithProperties, prop);
                }
            }
        }

        private void ValorObrigatorio(DBDataSource dbdts, object objWithProperties, System.Reflection.FieldInfo prop)
        {
            var propriedadeItemForm = (ItemForm)prop.GetValue(objWithProperties);
            var propriedadeInterface = (IItemFormObrigatorio)prop.GetValue(objWithProperties);
            for (int i = 0; i < dbdts.Size; i++)
            {
                var valor = dbdts.GetValue(propriedadeItemForm.Datasource, i).Trim();
                var fieldType = dbdts.Fields.Item(propriedadeItemForm.Datasource).Type;
                var valido = ValorEstaValido(valor, fieldType);

                if (!valido)
                {
                    var mensagem = !string.IsNullOrEmpty(propriedadeInterface.Mensagem) ? propriedadeInterface.Mensagem : $"Não foi definido uma mensagem para o itemformobrigatorio {propriedadeItemForm.ItemUID}";
                    throw new FormValidationException(mensagem, propriedadeItemForm.ItemUID, propriedadeInterface.AbaUID, i);
                }
            }
        }

        private void ValorObrigatorioEUnico(DBDataSource dbdts, object objWithProperties, System.Reflection.FieldInfo prop)
        {
            var valores = new List<string>() { };
            var propriedadeItemForm = (ItemForm)prop.GetValue(objWithProperties);
            var propriedadeInterface = (IItemFormObrigatorioUnico)prop.GetValue(objWithProperties);
            for (int i = 0; i < dbdts.Size; i++)
            {
                var valor = dbdts.GetValue(propriedadeItemForm.Datasource, i).Trim();
                var fieldType = dbdts.Fields.Item(propriedadeItemForm.Datasource).Type;
                var valido = ValorEstaValido(valor, fieldType);

                if (!valido)
                {
                    var mensagem = !string.IsNullOrEmpty(propriedadeInterface.MensagemQuandoObrigatorio) ? propriedadeInterface.MensagemQuandoObrigatorio : $"Não foi definido uma 'mensagem quando obrigatório' para o ItemFormObrigatorioUnico {propriedadeItemForm.ItemUID}";
                    throw new FormValidationException(mensagem, propriedadeItemForm.ItemUID, propriedadeInterface.AbaUID, i);
                }
                else
                {
                    if (valores.Contains(valor))
                    {
                        var mensagem = !string.IsNullOrEmpty(propriedadeInterface.MensagemQuandoUnico) ? propriedadeInterface.MensagemQuandoUnico : $"Não foi definido uma 'mensagem quando único' para o ItemFormObrigatorioUnico {propriedadeItemForm.ItemUID}";
                        throw new FormValidationException(mensagem, propriedadeItemForm.ItemUID, propriedadeInterface.AbaUID, i);
                    }
                    else
                    {
                        valores.Add(valor);
                    }
                }
            }
        }

        private bool ValorEstaValido(string valor, BoFieldsType fieldType)
        {
            switch (fieldType)
            {
                case BoFieldsType.ft_AlphaNumeric:
                case BoFieldsType.ft_Text:
                case BoFieldsType.ft_NotDefined:
                case BoFieldsType.ft_Date:
                    return !String.IsNullOrEmpty(valor);
                case BoFieldsType.ft_Integer:
                case BoFieldsType.ft_Float:
                case BoFieldsType.ft_ShortNumber:
                case BoFieldsType.ft_Quantity:
                case BoFieldsType.ft_Price:
                case BoFieldsType.ft_Rate:
                case BoFieldsType.ft_Measure:
                case BoFieldsType.ft_Sum:
                case BoFieldsType.ft_Percent:
                    return Helpers.ToDouble(valor) > 0;
                default:
                    return false;
            }
        }

        public static void ChangeFormMode(SAPbouiCOM.Form form)
        {
            if (form.Mode == BoFormMode.fm_OK_MODE)
            {
                form.Mode = BoFormMode.fm_UPDATE_MODE;
            }
        }

        public void CarregarDataSourceFormPai(string localFormUID, string fatherFormUID, string localMatrixUID, string dbdts_name_compartilhado)
        {
            var fatherForm = GetFormIfExists(fatherFormUID);
            if (fatherForm != null)
            {
                var fatherDbdts = GetDBDatasource(fatherForm, dbdts_name_compartilhado);

                using (var formCOM = new FormCOM(localFormUID))
                {
                    SAPbouiCOM.Form localForm = formCOM.Form;
                    ((Matrix)localForm.Items.Item(localMatrixUID).Specific).FlushToDataSource();
                    var localDbdts = GetDBDatasource(localForm, dbdts_name_compartilhado);

                    Copy(localDbdts, ref fatherDbdts);
                }
            }
        }

        public void CarregarDadosMatriz(SAPbouiCOM.Form localForm, string fatherFormUID, string localMatrixUID, string dbdts_name_compartilhado)
        {
            var localDbdts = GetDBDatasource(localForm, dbdts_name_compartilhado);

            using (var formCOM = new FormCOM(fatherFormUID))
            {
                SAPbouiCOM.Form fatherForm = formCOM.Form;
                var fatherDbdts = GetDBDatasource(fatherForm, dbdts_name_compartilhado);

                Copy(fatherDbdts, ref localDbdts);

                var mtx = ((Matrix)localForm.Items.Item(localMatrixUID).Specific);
                mtx.LoadFromDataSourceEx(true);
            }
        }

        #endregion
    }
}
