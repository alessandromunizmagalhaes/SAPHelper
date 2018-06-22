namespace SAPHelper
{
    public abstract class TabsForm
    {
        public string ActiveFolder(SAPbouiCOM.Form form)
        {
            var res = string.Empty;
            foreach (var field in GetType().GetFields())
            {
                if (field.FieldType == typeof(TabForm))
                {
                    var propriedade = (TabForm)field.GetValue(this);
                    if (propriedade.PaneLevel == form.PaneLevel)
                    {
                        res = propriedade.ItemUID;
                        break;
                    }
                }
            }
            return res;
        }
    }
}
