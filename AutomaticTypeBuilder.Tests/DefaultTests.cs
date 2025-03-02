using AutomaticTypeBuilder.Internals.Concrete;

namespace AutomaticTypeBuilder.Tests;


public class DefaultTests
{
    [Theory]
    [MemberData(nameof(InitLogicMap))]       
    public void AssignmentLogic_Returns_CorrectDelegates(Type key, Type expectedType)
    {
        var _defaultInitLogic = new Default().AssignmentLogic;

        var actualType = _defaultInitLogic[key].GetType();
        
        Assert.Equal(expected:expectedType, actual:actualType);    
    }

    [Fact]
    public void AssignmentLogic_AllDefaultLogic_Checked()
    {
        var defaultInitLogic = new Default().AssignmentLogic;

        Assert.Equal(expected:defaultInitLogic.Count, actual:InitLogicMap.Count());
    }

    public static IEnumerable<object[]> InitLogicMap =>
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
}