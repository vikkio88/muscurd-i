using System;
namespace Muscurdi.Exceptions;
public class MasterPasswordException : Exception
{
    public MasterPasswordException(string? message) : base(message) { }
}
