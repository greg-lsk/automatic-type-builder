namespace AutomaticTypeBuilder;


public interface ITypeRegistry
{
    public int Count { get; }

    public IEnumerable<Type> Types {get;}
    public IEnumerable<object> Values {get;}
    
    public (Type Type, object Value) InfoAt(int index);
}