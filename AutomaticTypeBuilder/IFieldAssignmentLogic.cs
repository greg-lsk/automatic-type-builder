using System.Collections.ObjectModel;

namespace AutomaticTypeBuilder;


public interface IFieldAssignmentLogic
{
    public IFieldAssignmentLogic When<T>(Func<T> initialize);

    public T? Initialize<T>();
}