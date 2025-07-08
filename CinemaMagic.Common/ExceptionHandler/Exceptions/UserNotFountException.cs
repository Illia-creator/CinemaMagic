namespace CinemaMagic.Common.ExceptionHandler.Exceptions;

public class UserNotFountException : Exception
{
    public UserNotFountException()
    {
        
    }

    public UserNotFountException(string message)
        : base(message)
    {
        
    }
}