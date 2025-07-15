using System.Linq.Expressions;
using MockLite.Core;

namespace MockLite;

public class Mock<T> where T : class
{
    private readonly MockInterceptor _interceptor = new();
    
    public Mock()
    {
        Object = ProxyGenerator.CreateProxy<T>(_interceptor);
    }
    
    public T Object { get; }

    public ISetup<T> Setup(Expression<Action<T>> expression)
    {
        return _interceptor.Setup(expression);
    }
    
    public ISetup<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> expression)
    {
        return _interceptor.Setup(expression);
    }
    
    public ISetupGetter<T, TProperty> SetupGet<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        return _interceptor.SetupGet(expression);
    }

    public ISetupSetter<T> SetupSet<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        return _interceptor.SetupSet(expression);
    }

    public ISetupSequence<T, TResult> SetupSequence<TResult>(Expression<Func<T, TResult>> expression)
    {
        return _interceptor.SetupSequence(expression);
    }
    
    public ISetupSequence<T> SetupSequence(Expression<Action<T>> expression)
    {
        return _interceptor.SetupSequence(expression);
    }
    
    public void Verify(Expression<Action<T>> expression, Times? times = null)
    {
        _interceptor.Verify(expression, times ?? Times.AtLeastOnce);
    }
    
    public void Verify<TResult>(Expression<Func<T, TResult>> expression, Times? times = null)
    {
        _interceptor.Verify(expression, times ?? Times.AtLeastOnce);
    }
}