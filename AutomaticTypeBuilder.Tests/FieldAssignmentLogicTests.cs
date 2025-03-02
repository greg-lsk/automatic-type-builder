using Moq;
using System.Collections.ObjectModel;
using AutomaticTypeBuilder.Internals.Abstract;
using AutomaticTypeBuilder.Internals.Concrete;

namespace AutomaticTypeBuilder.Tests;


public class FieldAssignmentLogicTests
{
    private static readonly int _mockedAssignedInt = 42;
    private static readonly string _mockedAssignedString = "Hellow";


    [Fact]
    public void When_RegistersCorrectly_TheAssignmentLogic()
    {
        DefaultAssignmentLogicSetup(out var defaultMock, out var assignmentLogic);

        var assignedGuid = Guid.NewGuid();
        assignmentLogic.When(() => assignedGuid);
        var retrievedGuid = assignmentLogic.Initialize<Guid>();

        Assert.Equal(expected:assignedGuid, actual:retrievedGuid);            
    }

    [Fact]
    public void When_Overrides_DefaultAssignmentLogic_WithCustom()
    {
        var customAssignedInt = 7;
        int customIntAssignment() => customAssignedInt;

        DefaultAssignmentLogicSetup(out var defaultMock, out var assignmentLogic);

        assignmentLogic.When(customIntAssignment);
        var actualInt = assignmentLogic.Initialize<int>();

        Assert.Equal(expected:customAssignedInt, actual:actualInt);            
    }

    [Fact]
    public void When_DoesNotOverride_AlreadyOverriden_Logic()
    {
        var customAssignedInt = 7;
        int customIntAssignment() => customAssignedInt;

        var customReassignedInt = customAssignedInt * 2;
        int customIntReassignment() => customReassignedInt;        

        DefaultAssignmentLogicSetup(out var defaultMock, out var assignmentLogic);

        assignmentLogic.When(customIntAssignment);
        assignmentLogic.When(customIntReassignment);        
        var actualInt = assignmentLogic.Initialize<int>();

        Assert.Equal(expected:customAssignedInt, actual:actualInt);            
    }    

    [Fact]    
    public void Initialize_Returns_Correctly_For_DefaultRegistedType()
    {
        DefaultAssignmentLogicSetup(out var defaultMock, out var assignmentLogic);

        var actualIntValue = assignmentLogic.Initialize<int>();
        var actualStringValue = assignmentLogic.Initialize<string>();

        Assert.Equal(expected:_mockedAssignedInt, actual:actualIntValue);
        Assert.Equal(expected:_mockedAssignedString, actual:actualStringValue);        
    }

  

    private static void DefaultAssignmentLogicSetup(out Mock<IDefault> defaultMock,
                                                    out IFieldAssignmentLogic defaultAssignmentLogic)
    {
        defaultMock = new Mock<IDefault>();
        DefaultAssignmentLogicSetup(in defaultMock);
        defaultAssignmentLogic = new FieldAssignmentLogic(defaultMock.Object);            
    }

    private static void DefaultAssignmentLogicSetup(in Mock<IDefault> defaultMock) 
    => defaultMock.Setup(m => m.AssignmentLogic).Returns(new ReadOnlyDictionary<Type, Delegate>
    (
        new Dictionary<Type, Delegate>
        {
            {typeof(int), () => _mockedAssignedInt},
            {typeof(string), () => _mockedAssignedString},
        }
    )); 
}