using SAPbouiCOM;

namespace SAPHelper
{
    public abstract class Form
    {

        public abstract string FormType { get; }

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

        #endregion


        #region :: Utils

        protected SAPbouiCOM.Form GetForm(ItemEvent pVal)
        {
            return Global.SBOApplication.Forms.Item(pVal.FormUID);
        }

        protected DBDataSource GetDBDatasource(SAPbouiCOM.Form form, string dbdts_name)
        {
            return form.DataSources.DBDataSources.Item(dbdts_name);
        }

        protected DBDataSource GetDBDatasource(ItemEvent pVal, string dbdts_name)
        {
            SAPbouiCOM.Form form = GetForm(pVal);
            return form.DataSources.DBDataSources.Item(dbdts_name);
        }

        protected string GetNextPrimaryKey(string tabela_com_arroba, string campo = "Code")
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

        protected void PopularComboBox(SAPbouiCOM.Form form, string comboUID, string sql = "")
        {
            ComboBox comboBox = ((ComboBox)form.Items.Item(comboUID).Specific);
            PopularComboBox(comboBox, sql);
        }

        protected void PopularComboBox(ComboBox comboBox, string sql = "")
        {
            comboBox.Item.DisplayDesc = true;

            var rs = Helpers.DoQuery(sql);

            while (!rs.EoF)
            {
                comboBox.ValidValues.Add(rs.Fields.Item(0).Value.ToString(), rs.Fields.Item(1).Value.ToString());

                rs.MoveNext();
            }
        }

        #endregion
    }
}
