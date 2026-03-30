namespace AsadaLisboaBackend.Services.Exceptions
{
    public class SendEmailException : Exception
    {
        public SendEmailException() : base() { }
        public SendEmailException(string message) : base(message) { }
        public SendEmailException(string message, Exception inner) : base(message, inner) { }
    }
}
