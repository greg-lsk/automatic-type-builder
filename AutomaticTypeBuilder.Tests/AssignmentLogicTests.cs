using Moq;
using System.Collections.ObjectModel;
using AutomaticTypeBuilder.Tests.Data;
using AutomaticTypeBuilder.Internals.Abstract;
using AutomaticTypeBuilder.Internals.Concrete;

namespace AutomaticTypeBuilder.Tests;


public class AssignmentLogicTests
{
    public static ReadOnlyDictionary<Type, Delegate> DefaultAssignmentLogic 
    => AssignmentLogicTestsData.DefaultLogic;
    
    public static TheoryData<IEnumerable<Type>, IEnumerable<object?>> ExpectedAssignedValuesMap 
    => AssignmentLogicTestsData.ExpectedAssignedValues_OnDefaultLogic;


    [Fact]
    public void Register_CorrectlyAdds_TheAssignmentLogic()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var assignedGuid = Guid.NewGuid();
        assignmentLogic.Register(() => assignedGuid);
        var retrievedGuid = assignmentLogic.Assign<Guid>();

        Assert.Equal(expected:assignedGuid, actual:retrievedGuid);            
    }

    [Fact]
    public void Register_Overrides_DefaultAssignmentLogic_WithCustom()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var customAssignedInt = 7;
        int customIntAssignment() => customAssignedInt;

        assignmentLogic.Register(customIntAssignment);
        var actualInt = assignmentLogic.Assign<int>();

        Assert.Equal(expected:customAssignedInt, actual:actualInt);            
    }

    [Fact]
    public void Register_DoesNotOverride_AlreadyOverriden_Logic()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);        

        var customAssignedInt = 7;
        int customIntAssignment() => customAssignedInt;

        var customReassignedInt = customAssignedInt * 2;
        int customIntReassignment() => customReassignedInt;        

        assignmentLogic.Register(customIntAssignment);
        assignmentLogic.Register(customIntReassignment);        
        var actualInt = assignmentLogic.Assign<int>();

        Assert.Equal(expected:customAssignedInt, actual:actualInt);            
    }    

    [Fact]    
    public void Assign_Returns_Correctly_For_DefaultRegistedType()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var actualIntValue = assignmentLogic.Assign<int>();
        var actualStringValue = assignmentLogic.Assign<string>();

        Assert.Equal(expected:Defaults.IntValue, actual:actualIntValue);
        Assert.Equal(expected:Defaults.StringValue, actual:actualStringValue);        
    }

    [Fact]    
    public void Assign_Returns_DefaultValue_For_UnregistedType()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var actualGuidValue = assignmentLogic.Assign<Guid>();

        Assert.Equal(expected:default, actual:actualGuidValue);        
    }

    [Theory]
    [MemberData(nameof(ExpectedAssignedValuesMap))]    
    public void AssignBatch_Correctly_Assigns_Values(IEnumerable<Type> providedTypes,
                                                     IEnumerable<object?> expectedValues)
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        assignmentLogic.AssignBatch(in providedTypes, out var values);

        Assert.Equal(expected:expectedValues, actual:values);        
    }

    [Fact]
    public void AssignRandomBatch_Correctly_Sets_TheFieldNumber()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var fieldLimit = 5;

        assignmentLogic.AssignRandomBatch(fieldLimit, out var actualValues, out var actualTypes);
        var actualTypesCount = actualTypes.Count();
        var actualValuesCount = actualValues.Count();
        
        Assert.Equal(expected:fieldLimit, actual: actualTypesCount);
        Assert.Equal(expected:fieldLimit, actual: actualValuesCount);        
    }

    [Fact]
    public void AssignRandomBatch_Only_Chooses_From_RegisteredTypes()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var fieldLimit = 5;
        IEnumerable<Type> registeredTypes = [typeof(int), typeof(string), typeof(Guid)];  
    
        assignmentLogic.Register(Guid.NewGuid);
        assignmentLogic.AssignRandomBatch(fieldLimit, out var actualValues, out var actualTypes);

        Assert.All(actualTypes, t => Assert.Contains(t, registeredTypes));
    }

    [Fact]
    public void AssignRandomBatch_AssignsValue_OfRegisteredType()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var fieldLimit = 5;
        IEnumerable<Type> registeredTypes = [typeof(int), typeof(string), typeof(Guid)];  
    
        assignmentLogic.Register(Guid.NewGuid);
        assignmentLogic.AssignRandomBatch(fieldLimit, out var actualValues, out var actualTypes);

        Assert.All(actualValues, v => Assert.Contains(v.GetType(), registeredTypes));                           
    }

    [Fact]
    public void AssignRandomBatch_ThrowsInvalidDataException_WithNegativeFieldCount()
    {
        DefaultMock(out var defaultMock);
        DefaultAssignmentLogicSetup(in defaultMock, out var assignmentLogic);

        var fieldLimit = -5;
    
        void act() => assignmentLogic.AssignRandomBatch(fieldLimit, out var actualValues, out var actualTypes);

        Assert.Throws<InvalidDataException>(act);                           
    }    
  

    private static void DefaultMock(out Mock<IDefault> defaultMock)
    {
        defaultMock = new Mock<IDefault>();            
    }

    private static void DefaultAssignmentLogicSetup(in Mock<IDefault> defaultMock,
                                                    out IAssignmentLogic defaultAssignmentLogic)
    {
        defaultMock.Setup(m => m.AssignmentLogic).Returns(DefaultAssignmentLogic);
        defaultAssignmentLogic = new AssignmentLogic(defaultMock.Object);
    }   
}