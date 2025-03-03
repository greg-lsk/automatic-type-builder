using AutomaticTypeBuilder.Internals.Concrete;
using Moq;

namespace AutomaticTypeBuilder.Tests;


public class InstantiationDataTests
{
    private static readonly int _mockFieldCount = 2; 
    
    private static IEnumerable<Type> _mockedTypes = [typeof(int), typeof(string)];
    private static IEnumerable<object?> _mockedValues = [42, "Hellow"];


    [Fact]
    public void Ctor_Correctly_Initializes_Values()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, _mockFieldCount);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, _mockFieldCount);

        Assert.Equal(expected:_mockedValues, actual:instantiationData.Values);
    }

    [Fact]
    public void Ctor_Correctly_Initializes_Types()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, _mockFieldCount);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, _mockFieldCount);

        Assert.Equal(expected:_mockedTypes, actual:instantiationData.Types);
    }

    [Fact]
    public void Ctor_ThrowsInvalidDataException_WithNegativeFieldCount()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic);
        InstantiationData act() => new(mockedAssignmentLogic.Object, -_mockFieldCount);

        Assert.Throws<InvalidDataException>(act);
    }    

    [Fact]
    public void Count_Correctly_Returs_NumberOfFields()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, _mockFieldCount);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, _mockFieldCount);

        Assert.Equal(expected:_mockedTypes.Count(), actual:instantiationData.FieldCount);
    }

    [Theory]
    [MemberData(nameof(DataAtIndex))]
    public void DataAt_Correctly_Returts_FieldInfo_FromIndex(int index, Type expectedType, object? expectedValue)
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, _mockFieldCount);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, _mockFieldCount);

        var actualInfo = instantiationData.DataAt(index);
        
        Assert.Equal(expected:expectedType, actual:actualInfo.Type);
        Assert.Equal(expected:expectedValue, actual:actualInfo.Value);        
    }    

    [Fact]        
    public void DataAt_IndexOutOfRangeException_WithNegative_IndexProvided()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, _mockFieldCount);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, _mockFieldCount);

        void act() => instantiationData.DataAt(-_mockFieldCount);
        
        Assert.Throws<IndexOutOfRangeException>(act);        
    }

    [Fact]        
    public void DataAt_IndexOutOfRangeException_WithGreaterThatCapacity_IndexProvided()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, _mockFieldCount);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, _mockFieldCount);

        void act() => instantiationData.DataAt(instantiationData.FieldCount*2);
        
        Assert.Throws<IndexOutOfRangeException>(act);        
    }


    private static void MockAssignmentLogicSetup(out Mock<IFieldAssignmentLogic> mockedAssignmentLogic,
                                                 int fieldCount = 5)
    {
        mockedAssignmentLogic = new Mock<IFieldAssignmentLogic>();
        mockedAssignmentLogic.Setup(m => m.Initialize(fieldCount, out _mockedValues, out _mockedTypes))
                             .Callback((int s, out IEnumerable<object?> assignedValues, out IEnumerable<Type> providedTypes) => 
                             {
                                assignedValues = _mockedValues;
                                providedTypes = _mockedTypes;                                
                             });
    }

    public static TheoryData<int, Type, object?> DataAtIndex = new()
    {
        {0, _mockedTypes.ElementAt(0), _mockedValues.ElementAt(0)},
        {1, _mockedTypes.ElementAt(1), _mockedValues.ElementAt(1)}
    };
}