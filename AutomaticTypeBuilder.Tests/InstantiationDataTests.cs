using AutomaticTypeBuilder.Internals.Concrete;
using AutomaticTypeBuilder.Tests.Data;
using Moq;

namespace AutomaticTypeBuilder.Tests;


public class InstantiationDataTests
{
    private static readonly int _expectedFieldCount = TestData.InitializationDataCount; 
    private static IEnumerable<Type> _expectedInitializedTypes = TestData.InitializationTypes;
    private static IEnumerable<object?> _expectedInitializedValues = TestData.InitializationValues;
    public static TheoryData<int, Type, object?> InitializationDataAtIndex => TestData.InitializationDataAtIndex;


    [Fact]
    public void Ctor_Correctly_Initializes_Values()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, _expectedFieldCount);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, _expectedFieldCount);

        Assert.Equal(expected:_expectedInitializedValues, actual:instantiationData.Values);
    }

    [Fact]
    public void Ctor_Correctly_Initializes_Types()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, _expectedFieldCount);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, _expectedFieldCount);

        Assert.Equal(expected:_expectedInitializedTypes, actual:instantiationData.Types);
    }

    [Fact]
    public void Ctor_ThrowsInvalidDataException_WithNegativeFieldCount()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic);
        InstantiationData act() => new(mockedAssignmentLogic.Object, _expectedFieldCount*(-1));

        Assert.Throws<InvalidDataException>(act);
    }    

    [Fact]
    public void Count_Correctly_Returs_NumberOfFields()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, _expectedFieldCount);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, _expectedFieldCount);

        Assert.Equal(expected:_expectedInitializedTypes.Count(), actual:instantiationData.FieldCount);
    }

    [Theory]
    [MemberData(nameof(InitializationDataAtIndex))]
    public void DataAt_Correctly_Returts_FieldInfo_FromIndex(int index, Type expectedType, object? expectedValue)
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, _expectedFieldCount);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, _expectedFieldCount);

        var actualInfo = instantiationData.DataAt(index);
        
        Assert.Equal(expected:expectedType, actual:actualInfo.Type);
        Assert.Equal(expected:expectedValue, actual:actualInfo.Value);        
    }    

    [Fact]        
    public void DataAt_IndexOutOfRangeException_WithNegative_IndexProvided()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, _expectedFieldCount);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, _expectedFieldCount);

        void act() => instantiationData.DataAt(-_expectedFieldCount);
        
        Assert.Throws<IndexOutOfRangeException>(act);        
    }

    [Fact]        
    public void DataAt_IndexOutOfRangeException_WithGreaterThatCapacity_IndexProvided()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, _expectedFieldCount);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, _expectedFieldCount);

        void act() => instantiationData.DataAt(instantiationData.FieldCount*2);
        
        Assert.Throws<IndexOutOfRangeException>(act);        
    }


    private static void MockAssignmentLogicSetup(out Mock<IFieldAssignmentLogic> mockedAssignmentLogic,
                                                 int fieldCount = 5)
    {
        mockedAssignmentLogic = new Mock<IFieldAssignmentLogic>();
        mockedAssignmentLogic.Setup(m => m.Initialize(fieldCount, out _expectedInitializedValues, out _expectedInitializedTypes))
                             .Callback((int s, out IEnumerable<object?> assignedValues, out IEnumerable<Type> providedTypes) => 
                             {
                                assignedValues = _expectedInitializedValues;
                                providedTypes = _expectedInitializedTypes;                                
                             });
    }
}