using System;

namespace FundooNotesApplication
{
    public class CustomException: Exception
    {
        
        public enum exceptionType
        {
            NULL,
            NOTFOUND,
            UNAUTHORIZED,
        }
        exceptionType type;
        public CustomException(exceptionType type,string message) :base(message) {

            this.type = type;
           
        }

        public CustomException(string message) : base(message)
        {
        }
    }
}
