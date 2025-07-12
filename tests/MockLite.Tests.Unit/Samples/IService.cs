namespace MockLite.Tests.Unit.Samples;

public interface IService
{
    int GetCount();
    
    decimal GetAveragePrice();
    
    Task<string> GetDataAsync(int id);
    
    Task SaveAsync(string data);
    
    bool IsValid(string input);
}