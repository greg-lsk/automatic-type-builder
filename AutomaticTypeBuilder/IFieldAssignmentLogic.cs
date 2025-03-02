namespace AutomaticTypeBuilder;


public interface IFieldAssignmentLogic
{
    public IFieldAssignmentLogic When<T>(Func<T> initialize);

    public T? Initialize<T>();
    public void Initialize(in IEnumerable<Type> types, out IEnumerable<object> values);
    public void Initialize(int fieldLimit, out IEnumerable<object> values, out IEnumerable<Type> types);
}