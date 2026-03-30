namespace AsadaLisboaBackend.Services.Exceptions
{
    public class UpdateObjectException : Exception
    {
        public UpdateObjectException() : base() { }
        public UpdateObjectException(string message) : base(message) { }
        public UpdateObjectException(string message, Exception inner) : base(message, inner) { }
    }
}
