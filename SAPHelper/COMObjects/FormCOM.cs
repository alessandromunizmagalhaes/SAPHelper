using System;

namespace SAPHelper
{
    public class FormCOM : IDisposable
    {
        private SAPbouiCOM.Form _form;
        public SAPbouiCOM.Form Form { get { return _form; } }

        public FormCOM(string formUID)
        {
            _form = Global.SBOApplication.Forms.Item(formUID);
        }

        public void Dispose()
        {
            Global.ReleaseObjectFromMemory(_form);
        }
    }
}
