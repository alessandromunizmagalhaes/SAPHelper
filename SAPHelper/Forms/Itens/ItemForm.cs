using SAPbouiCOM;
using System;

namespace SAPHelper
{
    public class ItemForm
    {
        public string ItemUID { get; set; }
        public string Datasource { get; set; }

        public void SetValorUserDataSource(SAPbouiCOM.Form form, object valor)
        {
            form.DataSources.UserDataSources.Item(Datasource).Value = valor.ToString();
        }

        /// <summary>
        /// para campos de ponto flutuante, usar o tipo double.
        /// </summary>
        /// <param name="dbdts"></param>
        /// <param name="valor"></param>
        /// <param name="row"></param>
        public void SetValorDBDatasource(DBDataSource dbdts, object valor, int row = 0)
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

        public T GetValorDBDatasource<T>(DBDataSource dbdts, int row = 0)
        {
            dynamic valor = dbdts.GetValue(Datasource, row).Trim();
            var type = dbdts.Fields.Item(Datasource).Type;
            switch (type)
            {
                case BoFieldsType.ft_NotDefined:
                case BoFieldsType.ft_Text:
                case BoFieldsType.ft_AlphaNumeric:
                    break;
                case BoFieldsType.ft_ShortNumber:
                case BoFieldsType.ft_Integer:
                    valor = Convert.ToInt32(valor);
                    break;
                case BoFieldsType.ft_Date:
                    valor = Helpers.ToDate(valor);
                    break;
                case BoFieldsType.ft_Float:
                case BoFieldsType.ft_Quantity:
                case BoFieldsType.ft_Price:
                case BoFieldsType.ft_Rate:
                case BoFieldsType.ft_Measure:
                case BoFieldsType.ft_Sum:
                case BoFieldsType.ft_Percent:
                    valor = Helpers.ToDouble(valor);
                    break;
            }

            return (T)Convert.ChangeType(valor, typeof(T));
        }
    }
}
