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

    [Fact]    
    public void Initialize_Returns_DefaultValue_For_UnregistedType()
    {
        DefaultAssignmentLogicSetup(out var defaultMock, out var assignmentLogic);

        var actualGuidValue = assignmentLogic.Initialize<Guid>();

        Assert.Equal(expected:default, actual:actualGuidValue);        
    }

    [Theory]
    [MemberData(nameof(ExpectedValuesMap))]    
    public void InitializeCorrectlyFillsTheValuesToCollection(IEnumerable<Type> providedTypes,
                                                              IEnumerable<object?> expectedValues)
    {
        DefaultAssignmentLogicSetup(out var defaultMock, out var assignmentLogic);

        assignmentLogic.Initialize(in providedTypes, out var values);

        Assert.Equal(expected:expectedValues, actual:values);        
    }

    [Fact]
    public void Initialize_Correctly_Sets_TheFieldNumber()
    {
        var fieldLimit = 5;
        DefaultAssignmentLogicSetup(out var defaultMock, out var assignmentLogic);

        assignmentLogic.Initialize(fieldLimit, out var actualValues, out var actualTypes);
        var actualTypesCount = actualTypes.Count();
        var actualValuesCount = actualValues.Count();
        
        Assert.Equal(expected:fieldLimit, actual: actualTypesCount);
        Assert.Equal(expected:fieldLimit, actual: actualValuesCount);        
    }

    [Fact]
    public void Initialize_OnlySetsTypes_FromRegistered()
    {
        var fieldLimit = 5;
        DefaultAssignmentLogicSetup(out var defaultMock, out var assignmentLogic);
        IEnumerable<Type> registeredTypes = [typeof(int), typeof(string), typeof(Guid)];  
    
        assignmentLogic.When(Guid.NewGuid);
        assignmentLogic.Initialize(fieldLimit, out var actualValues, out var actualTypes);

        Assert.All(actualTypes, t => Assert.Contains(t, registeredTypes));                           
    }

    [Fact]
    public void Initialize_SetsValues_OfCorrectTypes()
    {
        var fieldLimit = 5;
        DefaultAssignmentLogicSetup(out var defaultMock, out var assignmentLogic);
        IEnumerable<Type> registeredTypes = [typeof(int), typeof(string), typeof(Guid)];  
    
        assignmentLogic.When(Guid.NewGuid);
        assignmentLogic.Initialize(fieldLimit, out var actualValues, out var actualTypes);

        Assert.All(actualValues, v => Assert.Contains(v.GetType(), registeredTypes));                           
    }

    [Fact]
    public void Initialize_ThrowsInvalidDataException_WithNegativeFieldCount()
    {
        var fieldLimit = -5;
        DefaultAssignmentLogicSetup(out var defaultMock, out var assignmentLogic);
    
        void act() => assignmentLogic.Initialize(fieldLimit, out var actualValues, out var actualTypes);

        Assert.Throws<InvalidDataException>(act);                           
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

    public static TheoryData<IEnumerable<Type>, IEnumerable<object?>> ExpectedValuesMap = new()
    {
        {
            [typeof(int), typeof(string), typeof(Guid)],
            [_mockedAssignedInt, _mockedAssignedString, default(Guid)]
        },
        {
            [typeof(int), typeof(string), typeof(Guid), typeof(FieldAssignmentLogic)],
            [_mockedAssignedInt, _mockedAssignedString, default(Guid), default(FieldAssignmentLogic)]
        }      
    };      
}