using SAPbouiCOM;
using System;

namespace SAPHelper
{
    public class ItemForm
    {
        public string ItemUID { get; set; }
        public string Datasource { get; set; }

        public void SetaValorUserDataSource(SAPbouiCOM.Form form, object valor)
        {
            form.DataSources.UserDataSources.Item(Datasource).Value = valor.ToString();
        }

        public void SetaValorDBDatasource(DBDataSource dbdts, object valor, int row = 0)
        {
            var type = dbdts.Fields.Item(Datasource).Type;
            var valorFinal = string.Empty;
            switch (type)
            {
                case BoFieldsType.ft_Date:
                    if (valor.GetType() == typeof(DateTime))
                    {
                        valorFinal = Helpers.ToString((DateTime)valor);
                    }
                    else
                    {
                        valorFinal = valor.ToString();
                    }
                    break;
                case BoFieldsType.ft_Float:
                case BoFieldsType.ft_ShortNumber:
                case BoFieldsType.ft_Quantity:
                case BoFieldsType.ft_Price:
                case BoFieldsType.ft_Rate:
                case BoFieldsType.ft_Measure:
                case BoFieldsType.ft_Sum:
                case BoFieldsType.ft_Percent:
                    if (valor.GetType() == typeof(double))
                    {
                        valorFinal = Helpers.ToString((double)valor);
                    }
                    else
                    {
                        valorFinal = valor.ToString();
                    }
                    break;
                case BoFieldsType.ft_NotDefined:
                case BoFieldsType.ft_AlphaNumeric:
                case BoFieldsType.ft_Integer:
                case BoFieldsType.ft_Text:
                default:
                    valorFinal = valor.ToString();
                    break;
            }
            dbdts.SetValue(Datasource, row, valorFinal);
        }
    }
}
