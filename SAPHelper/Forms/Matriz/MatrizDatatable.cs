using SAPbouiCOM;

namespace SAPHelper
{
    public abstract class MatrizDatatable : MatrizForm
    {
        public void Bind(Matrix mtx)
        {
            var props = GetType().GetFields();
            foreach (var prop in props)
            {
                if (prop.FieldType == typeof(ItemForm))
                {
                    var propriedade = (ItemForm)prop.GetValue(this);
                    mtx.Columns.Item(propriedade.ItemUID).DataBind.Bind(Datasource, propriedade.Datasource);
                }
            }
        }

        public void Bind(SAPbouiCOM.Form form)
        {
            Bind(form.Items.Item(ItemUID).Specific);
        }
    }
}
