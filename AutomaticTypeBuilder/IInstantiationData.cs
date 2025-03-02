namespace AutomaticTypeBuilder;


public interface IInstantiationData
{
    public int FieldCount { get; }

    public IEnumerable<Type> Types {get;}
    public IEnumerable<object?> Values {get;}
    
    public (Type Type, object? Value) DataAt(int index);
}