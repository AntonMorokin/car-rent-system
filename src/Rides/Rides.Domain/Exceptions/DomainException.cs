namespace Rides.Domain.Exceptions;

public class DomainException : Exception
{
    public ErrorCodes? ErrorCode { get; }

    public DomainException(ErrorCodes errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    public DomainException(string message) : this(ErrorCodes.BusinessLogicViolation, message)
    {
    }
}