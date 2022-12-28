namespace Rides.Domain.Exceptions;

public class DomainException : Exception
{
    public int? ErrorCode { get; }

    public DomainException(string message) : base(message)
    {
    }

    public DomainException(int errorCode, string message) : this(message)
    {
        ErrorCode = errorCode;
    }
}