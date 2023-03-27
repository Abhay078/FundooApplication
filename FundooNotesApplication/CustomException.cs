using System;

namespace RepositoryLayer.Services
{
    public class CustomException : Exception
    {

        public CustomException() { }
        public CustomException(string message) : base(message) { }
        
    }
}
