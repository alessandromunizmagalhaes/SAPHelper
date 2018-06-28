using SAPbouiCOM;
using System;

namespace SAPHelper
{
    public class DBDatasourceCOM : IDisposable
    {
        private DBDataSource dbdts;
        public DBDataSource Dbdts { get { return dbdts; } }

        public DBDatasourceCOM(SAPbouiCOM.Form form, string dbdts_name)
        {
            dbdts = form.DataSources.DBDataSources.Item(dbdts_name);
        }

        public DBDatasourceCOM(string FormUID, string dbdts_name)
        {
            using (var formCOM = new FormCOM(FormUID))
            {
                dbdts = formCOM.Form.DataSources.DBDataSources.Item(dbdts_name);
            }
        }

        public void Dispose()
        {
            Global.ReleaseObjectFromMemory(dbdts);
        }
    }
}
