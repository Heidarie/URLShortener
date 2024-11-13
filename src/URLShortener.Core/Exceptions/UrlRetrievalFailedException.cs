namespace URLShortener.Core.Exceptions;

public class UrlRetrievalFailedException : Exception
{
    public UrlRetrievalFailedException() : base("An error occurred while retrieving a shortened url record.")
    {
    }
}