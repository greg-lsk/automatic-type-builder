using AutomaticTypeBuilder.Tests.Data;
using AutomaticTypeBuilder.Internals.Concrete;

namespace AutomaticTypeBuilder.Tests;


public class DefaultTests
{
    public static IEnumerable<object[]> TypeToFuncMap => TestData.TypeToFuncMap;


    [Theory]
    [MemberData(nameof(TypeToFuncMap))]       
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

        Assert.Equal(expected:defaultInitLogic.Count, actual:TypeToFuncMap.Count());
    }
}