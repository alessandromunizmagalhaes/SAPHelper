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
            PopularComboBox(comboBox);
        }

        public void PopularComboBox(ComboBox comboBox)
        {
            RemoverTodosValoresValidados(comboBox);
            AcrescentarValoresValidados(comboBox);
        }

        public void AcrescentarValoresValidados(ComboBox comboBox)
        {
            if (!string.IsNullOrEmpty(SQL))
            {
                var rs = Helpers.DoQuery(SQL);
                while (!rs.EoF)
                {
                    comboBox.ValidValues.Add(rs.Fields.Item(0).Value.ToString(), rs.Fields.Item(1).Value.ToString());

                    rs.MoveNext();
                }
            }
            else
            {
                throw new MissingFieldException($"Faltando a propriedade SQL do Item {ItemUID}");
            }
        }

        public void RemoverTodosValoresValidados(ComboBox comboBox)
        {
            var count = comboBox.ValidValues.Count;
            for (int i = 0; i < count; i++)
            {
                comboBox.ValidValues.Remove(0, BoSearchKey.psk_Index);
            }
        }

    }
}
