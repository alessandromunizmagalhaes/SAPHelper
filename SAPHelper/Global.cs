using SAPbouiCOM;
using System;

namespace SAPHelper
{
    public static class Global
    {
        public static Application SBOApplication;
        public static SAPbobsCOM.Company Company;

        public static void RecebeSBOApplication(Application application)
        {
            SBOApplication = application;
        }

        public static void RecebeCompany(SAPbobsCOM.Company company)
        {
            Company = company;
        }

        public static void ReleaseObjectFromMemory(object o)
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            if (o != null)
            {
                o = null;
            }
            GC.Collect();
        }
    }
}
