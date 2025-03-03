using Moq;
using System.Collections.ObjectModel;
using AutomaticTypeBuilder.Tests.Data;
using AutomaticTypeBuilder.Internals.Abstract;
using AutomaticTypeBuilder.Internals.Concrete;

namespace AutomaticTypeBuilder.Tests;


public class FieldAssignmentLogicTests
{
    public static TheoryData<IEnumerable<Type>, IEnumerable<object?>> ExpectedAssignedValuesMap 
    => TestData.ExpectedAssignedValuesMap;


    [Fact]
    public void When_RegistersCorrectly_TheAssignmentLogic()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var assignedGuid = Guid.NewGuid();
        assignmentLogic.When(() => assignedGuid);
        var retrievedGuid = assignmentLogic.Initialize<Guid>();

        Assert.Equal(expected:assignedGuid, actual:retrievedGuid);            
    }

    [Fact]
    public void When_Overrides_DefaultAssignmentLogic_WithCustom()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var customAssignedInt = 7;
        int customIntAssignment() => customAssignedInt;

        assignmentLogic.When(customIntAssignment);
        var actualInt = assignmentLogic.Initialize<int>();

        Assert.Equal(expected:customAssignedInt, actual:actualInt);            
    }

    [Fact]
    public void When_DoesNotOverride_AlreadyOverriden_Logic()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);        

        var customAssignedInt = 7;
        int customIntAssignment() => customAssignedInt;

        var customReassignedInt = customAssignedInt * 2;
        int customIntReassignment() => customReassignedInt;        

        assignmentLogic.When(customIntAssignment);
        assignmentLogic.When(customIntReassignment);        
        var actualInt = assignmentLogic.Initialize<int>();

        Assert.Equal(expected:customAssignedInt, actual:actualInt);            
    }    

    [Fact]    
    public void Initialize_Returns_Correctly_For_DefaultRegistedType()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var actualIntValue = assignmentLogic.Initialize<int>();
        var actualStringValue = assignmentLogic.Initialize<string>();

        Assert.Equal(expected:Constant.IntValue, actual:actualIntValue);
        Assert.Equal(expected:Constant.StringValue, actual:actualStringValue);        
    }

    [Fact]    
    public void Initialize_Returns_DefaultValue_For_UnregistedType()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var actualGuidValue = assignmentLogic.Initialize<Guid>();

        Assert.Equal(expected:default, actual:actualGuidValue);        
    }

    [Theory]
    [MemberData(nameof(ExpectedAssignedValuesMap))]    
    public void InitializeCorrectlyFillsTheValuesToCollection(IEnumerable<Type> providedTypes,
                                                              IEnumerable<object?> expectedValues)
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        assignmentLogic.Initialize(in providedTypes, out var values);

        Assert.Equal(expected:expectedValues, actual:values);        
    }

    [Fact]
    public void Initialize_Correctly_Sets_TheFieldNumber()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var fieldLimit = 5;

        assignmentLogic.Initialize(fieldLimit, out var actualValues, out var actualTypes);
        var actualTypesCount = actualTypes.Count();
        var actualValuesCount = actualValues.Count();
        
        Assert.Equal(expected:fieldLimit, actual: actualTypesCount);
        Assert.Equal(expected:fieldLimit, actual: actualValuesCount);        
    }

    [Fact]
    public void Initialize_OnlySetsTypes_FromRegistered()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var fieldLimit = 5;
        IEnumerable<Type> registeredTypes = [typeof(int), typeof(string), typeof(Guid)];  
    
        assignmentLogic.When(Guid.NewGuid);
        assignmentLogic.Initialize(fieldLimit, out var actualValues, out var actualTypes);

        Assert.All(actualTypes, t => Assert.Contains(t, registeredTypes));
    }

    [Fact]
    public void Initialize_SetsValues_OfCorrectTypes()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var fieldLimit = 5;
        IEnumerable<Type> registeredTypes = [typeof(int), typeof(string), typeof(Guid)];  
    
        assignmentLogic.When(Guid.NewGuid);
        assignmentLogic.Initialize(fieldLimit, out var actualValues, out var actualTypes);

        Assert.All(actualValues, v => Assert.Contains(v.GetType(), registeredTypes));                           
    }

    [Fact]
    public void Initialize_ThrowsInvalidDataException_WithNegativeFieldCount()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var fieldLimit = -5;
    
        void act() => assignmentLogic.Initialize(fieldLimit, out var actualValues, out var actualTypes);

        Assert.Throws<InvalidDataException>(act);                           
    }    
  

    private static void DefaultMock(out Mock<IDefault> defaultMock)
    {
        defaultMock = new Mock<IDefault>();            
    }

    private static void DefaultAssignmentLogicSetup(in Mock<IDefault> defaultMock,
                                                    out IFieldAssignmentLogic defaultAssignmentLogic)
    {
        defaultMock.Setup(m => m.AssignmentLogic).Returns(TestData.DefaultAssignmentLogic);
        defaultAssignmentLogic = new FieldAssignmentLogic(defaultMock.Object);
    }   
}