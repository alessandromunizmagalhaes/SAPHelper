using SAPbouiCOM;
using System;

namespace SAPHelper
{
    public class ComboForm : ItemForm
    {
        public string SQL { get; set; }

        public void PopularComboBox(SAPbouiCOM.Form form)
        {
            ComboBox comboBox = ((ComboBox)form.Items.Item(ItemUID).Specific);
            PopularComboBox(comboBox.ValidValues);
        }

        public void PopularComboBox(SAPbouiCOM.Form form, string matrixUID, string colunaUID)
        {
            PopularComboBox(((Matrix)form.Items.Item(matrixUID).Specific).Columns.Item(colunaUID).ValidValues);
        }

        public void PopularComboBox(Matrix mtx, string colunaUID)
        {
            PopularComboBox(mtx.Columns.Item(colunaUID).ValidValues);
        }

        public void PopularComboBox(Column coluna)
        {
            PopularComboBox(coluna.ValidValues);
        }

        public void PopularComboBox(ValidValues validValues)
        {
            RemoverTodosValoresValidados(validValues);
            AcrescentarValoresValidados(validValues);
        }

        public void AcrescentarValoresValidados(ValidValues validValues)
        {
            if (!string.IsNullOrEmpty(SQL))
            {
                var rs = Helpers.DoQuery(SQL);
                while (!rs.EoF)
                {
                    validValues.Add(rs.Fields.Item(0).Value.ToString(), rs.Fields.Item(1).Value.ToString());

                    rs.MoveNext();
                }
            }
            else
            {
                throw new MissingFieldException($"Faltando a propriedade SQL do Item {ItemUID}");
            }
        }

        public void RemoverTodosValoresValidados(ValidValues validValues)
        {
            var count = validValues.Count;
            for (int i = 0; i < count; i++)
            {
                validValues.Remove(0, BoSearchKey.psk_Index);
            }
        }

    }
}
