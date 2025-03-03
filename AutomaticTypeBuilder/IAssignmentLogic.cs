namespace AutomaticTypeBuilder;


public interface IAssignmentLogic
{
    public IAssignmentLogic Register<T>(Func<T> initialize);

    public T? Assign<T>();
    public void AssignBatch(in IEnumerable<Type> types, out IEnumerable<object?> values);
    public void AssignRandomBatch(int fieldLimit, out IEnumerable<object?> values, out IEnumerable<Type> types);
}