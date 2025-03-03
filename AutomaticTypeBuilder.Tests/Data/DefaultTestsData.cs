namespace AutomaticTypeBuilder.Tests.Data;


internal static class DefaultTestsData
{
    internal static IEnumerable<object[]> TypeToFuncMap =>
    [
        MapFor<int>(),
        MapFor<uint>(),
        MapFor<long>(),
        MapFor<ulong>(),        
        MapFor<float>(),
        MapFor<short>(),
        MapFor<ushort>(),
        MapFor<double>(),
        MapFor<decimal>(),

        MapFor<bool>(),

        MapFor<byte>(),
        MapFor<sbyte>(),

        MapFor<char>(),
        MapFor<string>(),

        MapFor<Guid>(),
        MapFor<TimeSpan>(),
        MapFor<DateTime>(),
    ];

    private static object[] MapFor<T>() => [typeof(T), typeof(Func<T>)];
}