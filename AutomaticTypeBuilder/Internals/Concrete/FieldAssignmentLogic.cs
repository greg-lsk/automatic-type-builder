using System.Collections.ObjectModel;
using AutomaticTypeBuilder.Internals.Abstract;

namespace AutomaticTypeBuilder.Internals.Concrete;


internal class FieldAssignmentLogic(IDefault defaultData) : IFieldAssignmentLogic
{
    private readonly IDefault _default = defaultData;

    private readonly Dictionary<Type, Delegate> _customLogic = [];

    private ReadOnlyDictionary<Type, Delegate> DefaultLogic => _default.AssignmentLogic;
    private IReadOnlyCollection<Type> RegisteredTypes => [.. _customLogic.Keys.Concat(DefaultLogic.Keys).Distinct()];


    public IFieldAssignmentLogic When<T>(Func<T> initialize)
    {
        if (_customLogic.ContainsKey(typeof(T))) return this;

        _customLogic.Add(typeof(T), initialize);
        return this;
    }


    public T? Initialize<T>()
    {
        _customLogic.TryGetValue(typeof(T), out var initialization);
        if (initialization is Func<T> customInitialization) return customInitialization();

        DefaultLogic.TryGetValue(typeof(T), out initialization);
        return initialization is Func<T> prebuiltInitialization ? prebuiltInitialization() : default;
    }

    public void Initialize(in IEnumerable<Type> types, out IEnumerable<object> values) => values = types.Select(type =>
    {
        var method = typeof(FieldAssignmentLogic).GetMethod(nameof(Initialize), Type.EmptyTypes)
                                                 ?.MakeGenericMethod(type);

        return method is not null ? method.Invoke(this, null) 
                                  : throw new InvalidOperationException("Tried to invoke a nonexistent method");
    });

    public void Initialize(int fieldLimit, out IEnumerable<object> values, out IEnumerable<Type> types)
    {
        var random = new Random();
        types = [..RegisteredTypes.OrderBy(x => random.Next()).Take(fieldLimit)];
        Initialize(in types, out values);        
    }
}