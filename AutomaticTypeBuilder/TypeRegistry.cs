namespace AutomaticTypeBuilder;


internal class TypeRegistry : ITypeRegistry
{
    private readonly List<(Type Type, object Value)> _registry = [];


    public int Count => _registry.Count;

    public void Register<T>(T value)
    {
        _registry.Add((typeof(T), value));
    }

    public Type TypeAt(int index) => _registry[index].Type;
    public object ValueAt(int index) => _registry[index].Value;
    
    public Type[] GetTypes() => [.._registry.Select(x => x.Type)];
    public object[] GetValues() => [.._registry.Select(x => x.Value)];


    public void Clear() => _registry.Clear();
}