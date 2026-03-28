namespace AsadaLisboaBackend.Services.Exceptions
{
    public class RegisterUserException : Exception
    {
        public RegisterUserException() : base() { }
        public RegisterUserException(string message) : base(message) { }
        public RegisterUserException(string message, Exception inner) : base(message, inner) { }
    }
}
