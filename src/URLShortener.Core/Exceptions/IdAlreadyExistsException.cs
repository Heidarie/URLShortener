namespace URLShortener.Core.Exceptions;

public class IdAlreadyExistsException : Exception
{
    public IdAlreadyExistsException(string id) : base($"Id {id} already exists.")
    {
    }
}