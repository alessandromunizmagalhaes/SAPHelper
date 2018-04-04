using SAPbouiCOM;

namespace SAPHelper
{
    public abstract class MatrizForm : ItemForm
    {
        public void CriarColunaSumAuto(SAPbouiCOM.Form form, string colunaUID)
        {
            ((Matrix)form.Items.Item(ItemUID).Specific).Columns.Item(colunaUID).ColumnSetting.SumType = BoColumnSumType.bst_Auto;
        }
    }
}
