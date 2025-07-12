namespace MockLite;

public interface ISetupSetter<T>
{
    void Throws<TException>()
        where TException : Exception, new();
    
    void Throws(Exception exception);
    
    ISetupSetter<T> Callback(Action callback);
    
    ISetupSetter<T> Callback(Action<object[]> callback);
}