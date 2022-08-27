namespace Muscurdi.Exceptions;
public class InvalidPasswordException: DbException
{
    public InvalidPasswordException(string? message) : base(message) { }
}
