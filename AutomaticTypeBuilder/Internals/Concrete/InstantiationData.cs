namespace AutomaticTypeBuilder.Internals.Concrete;


internal class InstantiationData: IInstantiationData
{
    private readonly IEnumerable<Type> _types;
    private readonly IEnumerable<object?> _values;

    public IEnumerable<Type> Types => _types;
    public IEnumerable<object?> Values => _values;


    public InstantiationData(IFieldAssignmentLogic assignmentLogic, int fieldCount = 5)
    {
        if(fieldCount < 0) throw new InvalidDataException();

        assignmentLogic.Initialize(fieldCount, out _values, out _types);
    }


    public int FieldCount => _types.Count();

    public (Type Type, object? Value) DataAt(int index)
    {
        if(index < 0 || index > _types.Count()) throw new IndexOutOfRangeException();

        return (_types.ElementAt(index), _values.ElementAt(index));
    }
}