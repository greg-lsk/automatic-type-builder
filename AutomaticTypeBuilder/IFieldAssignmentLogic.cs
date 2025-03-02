namespace AutomaticTypeBuilder;


public interface IFieldAssignmentLogic
{
    public IFieldAssignmentLogic When<T>(Func<T> initialize);

    public T? Initialize<T>();
    public void Initialize(out IEnumerable<object> values, in IEnumerable<Type> types);
    public void Initialize(out IEnumerable<object> values, out IEnumerable<Type> types, int fieldLimit);
}