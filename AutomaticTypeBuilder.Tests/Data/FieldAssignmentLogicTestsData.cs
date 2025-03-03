using System.Collections.ObjectModel;
using AutomaticTypeBuilder.Tests.Data.Dummy;

namespace AutomaticTypeBuilder.Tests.Data;


internal static class FieldAssignmentLogicTestsData
{
    internal static ReadOnlyDictionary<Type, Delegate> DefaultLogic => new
    (
        new Dictionary<Type, Delegate>
        {
            {typeof(int), () => Defaults.IntValue},
            {typeof(string), () => Defaults.StringValue}
        }
    );

    internal static TheoryData<IEnumerable<Type>, IEnumerable<object?>> ExpectedAssignedValues_OnDefaultLogic => new()
    {
        {
            [typeof(int), typeof(string), typeof(Guid)],
            [Defaults.IntValue, Defaults.StringValue, default(Guid)]
        },
        {
            [typeof(int), typeof(string), typeof(Guid), typeof(DummyClass)],
            [Defaults.IntValue, Defaults.StringValue, default(Guid), default(DummyClass)]
        }      
    };
}