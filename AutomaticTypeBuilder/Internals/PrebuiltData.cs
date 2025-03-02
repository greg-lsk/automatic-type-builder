using System.Collections.ObjectModel;

namespace AutomaticTypeBuilder.Internals;


internal static class PrebuiltData
{
    private static readonly Random _random = new();

    internal readonly static ReadOnlyDictionary<Type, Delegate> AssignmentLogic = new(_assignmentLogic);

    private readonly static Dictionary<Type, Delegate> _assignmentLogic = new()
    {
        [typeof(int)] = () => _random.Next(),
        [typeof(uint)] = () => (uint)_random.Next(),
        [typeof(long)] = () => (long)_random.Next() << 32 | (uint)_random.Next(),
        [typeof(ulong)] = () => (ulong)_random.Next() << 32 | (uint)_random.Next(),
        [typeof(float)] = () => (float)_random.NextDouble(),
        [typeof(short)] = () => (short)_random.Next(short.MinValue, short.MaxValue),
        [typeof(ushort)] = () => (ushort)_random.Next(ushort.MinValue, ushort.MaxValue),
        [typeof(double)] = () => _random.NextDouble(),
        [typeof(decimal)] = () => (decimal)_random.NextDouble(),

        [typeof(bool)] = () => _random.Next(0, 2) == 1,

        [typeof(byte)] = () => (byte)_random.Next(byte.MinValue, byte.MaxValue),
        [typeof(sbyte)] = () => (sbyte)_random.Next(sbyte.MinValue, sbyte.MaxValue),        

        [typeof(char)] = () => (char)_random.Next(char.MinValue, char.MaxValue),        
        [typeof(string)] = () => Guid.NewGuid().ToString(),
        
        [typeof(Guid)] = () => Guid.NewGuid(),
        [typeof(TimeSpan)] = () => TimeSpan.FromSeconds(_random.Next()),
        [typeof(DateTime)] = () => DateTime.Now.AddSeconds(_random.Next())
    };
}