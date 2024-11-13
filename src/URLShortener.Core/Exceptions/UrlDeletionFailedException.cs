namespace URLShortener.Core.Exceptions;

public class UrlDeletionFailedException : Exception
{
    public UrlDeletionFailedException() : base("An error occurred while deleting a shortened url record.")
    {
    }
}