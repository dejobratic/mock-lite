using System.Linq.Expressions;

namespace MockLite;

public interface ISetupGetter<T, in TProperty>
{
    void Returns(TProperty value);
    
    void Returns(Func<TProperty> valueFunction);
    
    void Throws<TException>()
        where TException : Exception, new();
    
    void Throws(Exception exception);
    
    ISetupGetter<T, TProperty> Callback(Action callback);
}