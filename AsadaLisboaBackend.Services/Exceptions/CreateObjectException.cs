namespace AsadaLisboaBackend.Services.Exceptions
{
    public class CreateObjectException : Exception
    {
        public CreateObjectException() : base() { }
        public CreateObjectException(string message) : base(message) { }
        public CreateObjectException(string message, Exception inner) : base(message, inner) { }
    }
}
