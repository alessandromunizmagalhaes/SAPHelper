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

        public static string ToSAPString(double valor)
        {
            //GetSBOBob().Format_MoneyToString(10, BoMoneyPrecisionTypes.mpt_Price);
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

        private static SBObob GetSBOBob()
        {
            return Global.Company.GetBusinessObject(BoObjectTypes.BoBridge);
        }

        public static Recordset GetRecordset()
        {
            return Global.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        }

        public static Recordset DoQuery(string sql)
        {
            var rs = GetRecordset();
            rs.DoQuery(sql);
            return rs;
        }

    }
}
