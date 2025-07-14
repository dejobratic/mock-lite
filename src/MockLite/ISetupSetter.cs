namespace MockLite;

public interface ISetupSetter<T>
{
    void Throws<TException>()
        where TException : Exception, new();
    
    void Throws(Exception exception);
    
    ISetupSetter<T> Callback(Action callback);
    
    ISetupSetter<T> Callback(Action<object[]> callback);
    
    ISetupSetter<T> Callback<T1>(Action<T1> callback);
    
    ISetupSetter<T> Callback<T1, T2>(Action<T1, T2> callback);
    
    ISetupSetter<T> Callback<T1, T2, T3>(Action<T1, T2, T3> callback);
    
    ISetupSetter<T> Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback);
}