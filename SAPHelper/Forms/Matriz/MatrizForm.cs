using SAPbouiCOM;

namespace SAPHelper
{
    public abstract class MatrizForm : ItemForm
    {
        public void CriarColunaSumAuto(SAPbouiCOM.Form form, string colunaUID)
        {
            ((Matrix)form.Items.Item(ItemUID).Specific).Columns.Item(colunaUID).ColumnSetting.SumType = BoColumnSumType.bst_Auto;
        }

        public void ValidarCamposObrigatorios(DBDataSource dbdts)
        {
            var props = GetType().GetFields();
            foreach (var prop in props)
            {
                if (typeof(IItemFormObrigatorio).IsAssignableFrom(prop.FieldType))
                {
                    var propriedadeItemForm = (ItemForm)prop.GetValue(this);
                    var propriedadeInterface = (IItemFormObrigatorio)prop.GetValue(this);
                    for (int i = 0; i < dbdts.Size; i++)
                    {
                        var valor = dbdts.GetValue(propriedadeItemForm.Datasource, i).Trim();

                        if (string.IsNullOrEmpty(valor))
                        {
                            string mensagem = !string.IsNullOrEmpty(propriedadeInterface.Mensagem) ? propriedadeInterface.Mensagem : $"Não foi definido uma mensagem para o itemformobrigatorio {propriedadeItemForm.ItemUID}";
                            throw new FormValidationException(mensagem, propriedadeItemForm.ItemUID, propriedadeInterface.AbaUID, i);
                        }
                    }
                }
            }
        }

        protected void ClicarNaUltimaLinha(Matrix mtx)
        {
            for (int i = 0; i < mtx.Columns.Count; i++)
            {
                if (mtx.Columns.Item(i).Editable)
                {
                    mtx.Columns.Item(i).Cells.Item(mtx.RowCount).Click();
                    break;
                }
            }
        }
    }
}
