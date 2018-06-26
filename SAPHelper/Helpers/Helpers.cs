using SAPbobsCOM;
using System;
namespace SAPHelper
{
    public static class Helpers
    {
        public static string ToString(DateTime date)
        {
            return date.ToString("yyyyMMdd");
        }

        /// <summary>
        /// Geralmente utilizado para lidar com campo, onde a vírgula é importante como casa decimal e deve ser substituida por ponto
        /// Ou então para atualizar valor em campo SAP
        /// </summary>
        /// <param name="valor">1.234,56</param>
        /// <returns>1234.56</returns>
        public static string ToString(double valor)
        {
            return valor.ToString().Replace(".", "").Replace(",", ".");
        }

        public static DateTime ToDate(string date)
        {
            SBObob sBObob = GetSBOBob();
            return sBObob.Format_StringToDate(date).Fields.Item(0).Value;
        }

        public static double ToDouble(string value)
        {
            return Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static int ToInt(string value)
        {
            return Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        private static SBObob GetSBOBob()
        {
            return Global.Company.GetBusinessObject(BoObjectTypes.BoBridge);
        }

        public static Recordset GetRecordset()
        {
            return Global.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        }

        /*
        public static Recordset DoQuery(string sql)
        {
            var rs = GetRecordset();
            rs.DoQuery(sql);
            return rs;
        }
        */
    }
}
