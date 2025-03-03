namespace AutomaticTypeBuilder.Tests.Data;

internal static class InstantiationDataTestsData
{

    internal static int FieldCount => 2;
    internal static IEnumerable<Type> FieldTypes = [typeof(int), typeof(string)];
    public static IEnumerable<object?> AssignedValues = [Defaults.IntValue, Defaults.StringValue];
             
    public static TheoryData<int, Type, object?> InitializationDataAtIndex = new()
    {
        {0, FieldTypes.ElementAt(0), AssignedValues.ElementAt(0)},
        {1, FieldTypes.ElementAt(1), AssignedValues.ElementAt(1)}
    };       
}