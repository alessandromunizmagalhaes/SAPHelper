using SAPbouiCOM;
using System;

namespace SAPHelper
{
    public abstract class MatrizMasterDataForm : MatrizForm
    {
        public void AdicionarLinha(SAPbouiCOM.Form form)
        {
            try
            {
                form.Freeze(true);
                var mtx = ((Matrix)form.Items.Item(ItemUID).Specific);
                mtx.AddRow();
                mtx.ClearRowData(mtx.RowCount);
            }
            finally
            {
                form.Freeze(false);
            }
        }

        public void RemoverLinha(SAPbouiCOM.Form form)
        {
            var mtx = ((Matrix)form.Items.Item(ItemUID).Specific);

            int row = mtx.GetNextSelectedRow();
            if (row > 0)
            {
                mtx.DeleteRow(row);
            }
            else
            {
                Dialogs.PopupError("Selecione uma linha.");
            }
        }

        public void CarregarDados(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            throw new NotImplementedException();
        }

        private void ReArrangeLineId(DBDataSource dbdts)
        {
            var lineIDField = dbdts.Fields.Item("LineId");
            if (lineIDField != null)
            {
                for (int i = 0; i < dbdts.Size; i++)
                {
                    dbdts.SetValue("LineId", i, (i + 1).ToString());
                }
            }
        }
    }
}
