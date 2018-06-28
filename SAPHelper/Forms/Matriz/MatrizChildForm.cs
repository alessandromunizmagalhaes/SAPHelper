﻿using SAPbouiCOM;
using System;

namespace SAPHelper
{
    public abstract class MatrizChildForm : MatrizForm
    {
        public void AdicionarLinha(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            var mtx = ((Matrix)form.Items.Item(ItemUID).Specific);

            mtx.FlushToDataSource();
            try
            {
                form.Freeze(true);

                dbdts.InsertRecord(dbdts.Size);
                ReArrangeLineId(dbdts);
                mtx.LoadFromDataSourceEx();

                ClicarNaUltimaLinha(mtx);
            }
            catch (Exception e)
            {
                Dialogs.Error("Erro ao tentar adicionar uma nova linha a matriz.\nErro: " + e.Message);
            }
            finally
            {
                form.Freeze(false);
            }
        }

        public void RemoverLinhaSelecionada(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            var mtx = ((Matrix)form.Items.Item(ItemUID).Specific);

            int row = mtx.GetNextSelectedRow();
            if (row > 0)
            {
                mtx.DeleteRow(row);
                mtx.FlushToDataSource();

                ReArrangeLineId(dbdts);
                mtx.LoadFromDataSourceEx();

                Form.ChangeFormMode(form);
            }
            else
            {
                Dialogs.PopupError("Selecione uma linha.");
            }
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
