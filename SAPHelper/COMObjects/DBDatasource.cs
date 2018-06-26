using SAPbouiCOM;
using System;

namespace SAPHelper
{
    public class DBDatasource : IDisposable
    {
        private DBDataSource dbdts;

        public DBDataSource GetDBDatasource(SAPbouiCOM.Form form, string dbdts_name)
        {
            return form.DataSources.DBDataSources.Item(dbdts_name);
        }

        public void Dispose()
        {
            Global.ReleaseObjectFromMemory(dbdts);
        }
    }
}
