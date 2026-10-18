namespace AsadaLisboaBackend.Services.Exceptions;

public class InUsedException : Exception
{
    public InUsedException() { }
    public InUsedException(string message) : base(message) { }
    public InUsedException(string message, Exception inner) : base(message, inner) { }
}