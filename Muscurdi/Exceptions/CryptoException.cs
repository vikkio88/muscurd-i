using System;
namespace Muscurdi.Exceptions;
public class CryptoException : Exception
{
    public CryptoException(string? message) : base(message) { }
}
