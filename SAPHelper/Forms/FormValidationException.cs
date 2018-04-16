using System;

namespace SAPHelper
{
    public class FormValidationException : Exception
    {
        private string _campo;
        public string Campo { get { return _campo; } }

        private string _abaUID;
        public string AbaUID { get { return _abaUID; } }

        private int _datasourceRow;
        public int DatasourceRow { get { return _datasourceRow; } }

        public FormValidationException(string message, string campo, string abaUID, int datasourceRow = 0) : base(message)
        {
            _campo = campo;
            _abaUID = abaUID;
            _datasourceRow = datasourceRow;
        }
    }
}