using System.Collections.ObjectModel;

namespace AutomaticTypeBuilder.Internals;


internal class FieldAssignmentLogic : IFieldAssignmentLogic
{
    private readonly Dictionary<Type, Delegate> _customLogic = [];
    private readonly ReadOnlyDictionary<Type, Delegate> _prebuildLogic = PrebuiltData.AssignmentLogic;
    
    private IReadOnlyCollection<Type> RegisteredTypes => [.. _customLogic.Keys.Concat(_prebuildLogic.Keys).Distinct()];


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

    public void Initialize(out IEnumerable<object> values, in IEnumerable<Type> types) => values = types.Select(type =>
    {
        var method = typeof(FieldAssignmentLogic).GetMethod(nameof(Initialize), Type.EmptyTypes)
                                                 ?.MakeGenericMethod(type);

        return method?.Invoke(this, null) ?? throw new InvalidOperationException("Tried to invoke a nonexistent method");
    });

    public void Initialize(out IEnumerable<object> values, out IEnumerable<Type> types, int fieldLimit)
    {
        var random = new Random();
        types = [..RegisteredTypes.OrderBy(x => random.Next()).Take(fieldLimit)];
        Initialize(out values, in types);        
    }
}