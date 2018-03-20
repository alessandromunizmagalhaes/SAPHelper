using System;

namespace SAPHelper
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message)
        {

        }
    }
}
