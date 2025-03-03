using AutomaticTypeBuilder.Internals.Concrete;
using Moq;

namespace AutomaticTypeBuilder.Tests;


public class InstantiationDataTests
{
    private static IEnumerable<Type> _mockedTypes = [typeof(int), typeof(string)];
    private static IEnumerable<object?> _mockedValues = [42, "Hellow"];


    [Fact]
    public void Ctor_Correctly_Initializes_Values()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, 2);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, 2);

        Assert.Equal(expected:_mockedValues, actual:instantiationData.Values);
    }

    [Fact]
    public void Ctor_Correctly_Initializes_Types()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, 2);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, 2);

        Assert.Equal(expected:_mockedTypes, actual:instantiationData.Types);
    }

    [Fact]
    public void Ctor_ThrowsInvalidDataException_WithNegativeFieldCount()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic);
        InstantiationData act() => new(mockedAssignmentLogic.Object, -5);

        Assert.Throws<InvalidDataException>(act);
    }    

    [Fact]
    public void Count_Correctly_Returs_NumberOfFields()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, 2);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, 2);

        Assert.Equal(expected:_mockedTypes.Count(), actual:instantiationData.FieldCount);
    }

    [Theory]
    [InlineData(0, typeof(int), 42)]
    [InlineData(1, typeof(string), "Hellow")]
    public void DataAt_Correctly_Returts_FieldInfo_FromIndex(int index, Type expectedType, object? expectedValue)
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, 2);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, 2);

        var actualInfo = instantiationData.DataAt(index);
        
        Assert.Equal(expected:expectedType, actual:actualInfo.Type);
        Assert.Equal(expected:expectedValue, actual:actualInfo.Value);        
    }    

    [Fact]        
    public void DataAt_IndexOutOfRangeException_WithNegative_IndexProvided()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, 2);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, 2);

        void act() => instantiationData.DataAt(-5);
        
        Assert.Throws<IndexOutOfRangeException>(act);        
    }

    [Fact]        
    public void DataAt_IndexOutOfRangeException_WithGreaterThatCapacity_IndexProvided()
    {
        MockAssignmentLogicSetup(out var mockedAssignmentLogic, 2);
        var instantiationData = new InstantiationData(mockedAssignmentLogic.Object, 2);

        void act() => instantiationData.DataAt(instantiationData.FieldCount+5);
        
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
}