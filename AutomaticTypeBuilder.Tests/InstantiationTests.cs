using Moq;
using AutomaticTypeBuilder.Tests.Data;
using AutomaticTypeBuilder.Internals.Concrete;
using AutomaticTypeBuilder.Tests.Data.Dummy;

namespace AutomaticTypeBuilder.Tests;


public class InstantiationTests
{

    [Fact]
    public void For_Returns_TheCorrect_Delegate()
    {
        InstantiationDataMock(out var instantionDataMock);
        InstantiationDataMockCollectionsSetup(in instantionDataMock);
        var instantiation = new Instantiation();

        var del = instantiation.For<InstantiationTestDummyClass>(instantionDataMock.Object);   

        Assert.IsType<Instantiate<InstantiationTestDummyClass>>(del);
    }

    [Fact]
    public void For_ReturnedDelegate_Executes_Correctly()
    {
        InstantiationDataMock(out var instantionDataMock);
        InstantiationDataMockCollectionsSetup(in instantionDataMock);
        var instantiation = new Instantiation();

        var del = instantiation.For<InstantiationTestDummyClass>(instantionDataMock.Object);
        var delegateResult = del();
           
        object?[] actualValues = [delegateResult.Field01, delegateResult.Field02, delegateResult.Field03];

        Assert.Equal(expected: InstantiationTestsData.DummyClassAssignedValues, actual:actualValues); 
    }    


    private static void InstantiationDataMock(out Mock<IInstantiationData> instantionDataMock)
    {
        instantionDataMock = new();
    }

    private static void InstantiationDataMockCollectionsSetup(in Mock<IInstantiationData> instantionDataMock)
    {
        instantionDataMock.Setup(m => m.Values).Returns(InstantiationTestsData.DummyClassAssignedValues);
        instantionDataMock.Setup(m => m.Types).Returns(InstantiationTestsData.DummyClassFieldTypes);
    }
}