using AutomaticTypeBuilder.Tests.Data.Dummy;

namespace AutomaticTypeBuilder.Tests.Data;


internal static class InstantiationTestsData
{
    internal static IEnumerable<Type> DummyClassFieldTypes => InstantiationTestDummyClass.FieldTypeBlueprint();
    internal static IEnumerable<object?> DummyClassAssignedValues => InstantiationTestDummyClass.FieldAssignedValues();
}