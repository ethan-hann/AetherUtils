namespace AetherUtils.Core.Exceptions;

public class QrException : Exception
{
    public QrException(string message) : base(message) { }
    public QrException(string message, Exception innerException) : base(message, innerException) { }
}