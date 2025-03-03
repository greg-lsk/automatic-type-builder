using AutomaticTypeBuilder.Internals.Concrete;
using AutomaticTypeBuilder.Tests.Data;

namespace AutomaticTypeBuilder.Tests;


public class DefaultTests
{
    [Theory]
    [MemberData(nameof(Data.Data.TypeToFuncMap))]       
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
}