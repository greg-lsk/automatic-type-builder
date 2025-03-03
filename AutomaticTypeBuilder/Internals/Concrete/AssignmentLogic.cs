using System.Collections.ObjectModel;
using AutomaticTypeBuilder.Internals.Abstract;

namespace AutomaticTypeBuilder.Internals.Concrete;


internal class AssignmentLogic(IDefault defaultData) : IAssignmentLogic
{
    private readonly IDefault _default = defaultData;

    private readonly Dictionary<Type, Delegate> _customAssignmentLogic = [];
    private ReadOnlyDictionary<Type, Delegate> DefaultLogic => _default.AssignmentLogic;

    private IReadOnlyCollection<Type> RegisteredTypes => [.. _customAssignmentLogic.Keys.Concat(DefaultLogic.Keys).Distinct()];


    public IAssignmentLogic Register<T>(Func<T> initialize)
    {
        if (_customAssignmentLogic.ContainsKey(typeof(T))) return this;

        _customAssignmentLogic.Add(typeof(T), initialize);
        return this;
    }


    public T? Assign<T>()
    {
        _customAssignmentLogic.TryGetValue(typeof(T), out var initialization);
        if (initialization is Func<T> customInitialization) return customInitialization();

        DefaultLogic.TryGetValue(typeof(T), out initialization);
        return initialization is Func<T> defaultInitialization ? defaultInitialization() : default;
    }

    public void AssignBatch(in IEnumerable<Type> types, out IEnumerable<object?> values) => values = types.Select(type =>
    {
        var method = typeof(AssignmentLogic).GetMethod(nameof(Assign), Type.EmptyTypes)
                                            ?.MakeGenericMethod(type);

        return method is not null ? method.Invoke(this, null) 
                                  : throw new InvalidOperationException("Tried to invoke a nonexistent method");
    });

    public void AssignRandomBatch(int fieldCount, out IEnumerable<object?> values, out IEnumerable<Type> types)
    {
        if(fieldCount < 0) throw new InvalidDataException("fieldLimit must be greater than zero");
        
        var random = new Random();
        types = Enumerable.Range(0, fieldCount)
                          .Select( _ => RegisteredTypes.ElementAt(random.Next(RegisteredTypes.Count)));
        AssignBatch(in types, out values);        
    }
}