using AutomaticTypeBuilder.Tests.Data.Dummy;

namespace AutomaticTypeBuilder.Tests.Data;


public static class TheoryData
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

        [typeof(Guid), typeof(Func<Guid>)],
        [typeof(TimeSpan), typeof(Func<TimeSpan>)],
        [typeof(DateTime), typeof(Func<DateTime>)]
    ];

    public static TheoryData<IEnumerable<Type>, IEnumerable<object?>> ExpectedAssignedValuesMap = new()
    {
        {
            [typeof(int), typeof(string), typeof(Guid)],
            [Constant.IntValue, Constant.StringValue, default(Guid)]
        },
        {
            [typeof(int), typeof(string), typeof(Guid), typeof(DummyClass)],
            [Constant.IntValue, Constant.StringValue, default(Guid), default(DummyClass)]
        }      
    };

    public static int InitializationDataCount => 2;
    public static IEnumerable<Type> InitializationTypes = [typeof(int), typeof(string)];
    public static IEnumerable<object?> InitializationValues = [Constant.IntValue, Constant.StringValue];         
    public static TheoryData<int, Type, object?> InitializationDataAtIndex = new()
    {
        {0, InitializationTypes.ElementAt(0), InitializationValues.ElementAt(0)},
        {1, InitializationTypes.ElementAt(1), InitializationValues.ElementAt(1)}
    };             
}