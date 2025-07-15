namespace MockLite.Exceptions;

public class MockException : Exception
{
    public MockException(string message)
        : base(message) { }
    
    public MockException(string message, Exception innerException)
        : base(message, innerException) { }
}