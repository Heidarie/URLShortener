namespace URLShortener.Core.Exceptions;

public class UrlCreationFailedException : Exception
{
    public UrlCreationFailedException() : base("An error occurred while creating a shortened url record.")
    {
    }
}