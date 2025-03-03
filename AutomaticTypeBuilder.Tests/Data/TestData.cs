using System.Collections.ObjectModel;
using AutomaticTypeBuilder.Tests.Data.Dummy;

namespace AutomaticTypeBuilder.Tests.Data;


public static class TestData
{
    public static IEnumerable<object[]> TypeToFuncMap =>
    [
        [typeof(int), typeof(Func<int>)],
        [typeof(uint), typeof(Func<uint>)],
        [typeof(long), typeof(Func<long>)],
        [typeof(ulong), typeof(Func<ulong>)],
        [typeof(float), typeof(Func<float>)],
        [typeof(short), typeof(Func<short>)],
        [typeof(ushort), typeof(Func<ushort>)],
        [typeof(double), typeof(Func<double>)],
        [typeof(decimal), typeof(Func<decimal>)],

        [typeof(bool), typeof(Func<bool>)],

        [typeof(byte), typeof(Func<byte>)],
        [typeof(sbyte), typeof(Func<sbyte>)],

        [typeof(char), typeof(Func<char>)],
        [typeof(string), typeof(Func<string>)],

        [typeof(TimeSpan), typeof(Func<TimeSpan>)],
        [typeof(DateTime), typeof(Func<DateTime>)]
    ];


    public static ReadOnlyDictionary<Type, Delegate> DefaultAssignmentLogic => new
    (
        new Dictionary<Type, Delegate>
        {
            {typeof(int), () => Defaults.IntValue},
            {typeof(string), () => Defaults.StringValue}
        }
    );
    public static TheoryData<IEnumerable<Type>, IEnumerable<object?>> ExpectedAssignedValuesMap = new()
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


    public static int InitializationDataCount => 2;
    public static IEnumerable<Type> InitializationTypes = [typeof(int), typeof(string)];
    public static IEnumerable<object?> InitializationValues = [Defaults.IntValue, Defaults.StringValue];         
    public static TheoryData<int, Type, object?> InitializationDataAtIndex = new()
    {
        {0, InitializationTypes.ElementAt(0), InitializationValues.ElementAt(0)},
        {1, InitializationTypes.ElementAt(1), InitializationValues.ElementAt(1)}
    };             
}