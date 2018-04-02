using System;

namespace SAPHelper
{
    public class FormValidationException : Exception
    {
        private string _campo;
        public string Campo { get { return _campo; } }
        public FormValidationException(string message, string campo) : base(message)
        {
            _campo = campo;
        }
    }
}