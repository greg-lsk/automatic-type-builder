namespace AutomaticTypeBuilder.Tests.Data.Dummy;


 internal class InstantiationTestDummyClass(int arg01, string arg02, Guid arg03)
{
    internal int Field01 {get;} = arg01;
    internal string Field02 {get;} = arg02;
    internal Guid Field03 {get;} = arg03;

    internal static IEnumerable<Type> FieldTypeBlueprint() => [typeof(int), typeof(string), typeof(Guid)];
    internal static IEnumerable<object?> FieldAssignedValues() => [42, "Dummy-Class", _testGuid];

    private static readonly Guid _testGuid = Guid.NewGuid();
}