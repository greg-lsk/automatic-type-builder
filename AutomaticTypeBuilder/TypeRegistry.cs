namespace AutomaticTypeBuilder;


internal class TypeRegistry: ITypeRegistry
{
    private readonly IEnumerable<Type> _types;
    private readonly IEnumerable<object> _values;

    public IEnumerable<Type> Types => _types;
    public IEnumerable<object> Values => _values;


    public TypeRegistry(IFieldAssignmentLogic assignmentLogic, int fieldCount = 5)
    {
        assignmentLogic.Initialize(out _values, out _types, fieldCount);
    }


    public int Count => _types.Count();
    
    public (Type Type, object Value) InfoAt(int index) => (_types.ElementAt(index), _values.ElementAt(index));
}