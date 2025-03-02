using System.Collections.ObjectModel;

namespace AutomaticTypeBuilder.Internals;


internal class FieldAssignmentLogic : IFieldAssignmentLogic
{
    private readonly Dictionary<Type, Delegate> _customLogic = [];
    private readonly ReadOnlyDictionary<Type, Delegate> _prebuildLogic = PrebuiltData.AssignmentLogic;
    

    public IFieldAssignmentLogic When<T>(Func<T> initialize)
    {
        _customLogic.Add(typeof(T), initialize);
        return this;
    }

    public T? Initialize<T>()
    {
        _customLogic.TryGetValue(typeof(T), out var initialization);
        if (initialization is Func<T> customInitialization) return customInitialization();

        _prebuildLogic.TryGetValue(typeof(T), out initialization);
        return initialization is Func<T> prebuiltInitialization ? prebuiltInitialization() : default;
    }
}