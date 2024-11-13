namespace URLShortener.Core.Exceptions;

public class IdPatternNotMatchedException : Exception
{
    public IdPatternNotMatchedException(string id) : base($"Id {id} is not an alphanumeric.")
    {
    }
}