using SAPbobsCOM;
using System;

namespace SAPHelper
{
    public class RecordSet : IDisposable
    {
        private Recordset rs;

        public RecordSet()
        {
            rs = Global.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        }

        public Recordset DoQuery(string sql)
        {
            rs.DoQuery(sql);
            return rs;
        }

        public void Dispose()
        {
            Global.ReleaseObjectFromMemory(rs);
        }
    }
}
