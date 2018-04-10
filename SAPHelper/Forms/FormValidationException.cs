using System;

namespace SAPHelper
{
    public class FormValidationException : Exception
    {
        private string _campo;
        public string Campo { get { return _campo; } }

        private string _abaUID;
        public string AbaUID { get { return _abaUID; } }
        public FormValidationException(string message, string campo, string abaUID) : base(message)
        {
            _campo = campo;
            _abaUID = abaUID;
        }
    }
}