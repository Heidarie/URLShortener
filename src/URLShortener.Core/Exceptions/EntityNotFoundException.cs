namespace URLShortener.Core.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string id) : base($"Short URL with id {id} not found.")
    {
    }
}