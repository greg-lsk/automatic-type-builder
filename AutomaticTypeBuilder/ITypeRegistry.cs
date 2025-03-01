namespace AutomaticTypeBuilder;


public interface ITypeRegistry
{
    public int Count { get; }

    public void Register<T>(T value);

    public Type TypeAt(int index);
    public object ValueAt(int index);

    public Type[] GetTypes();
    public object[] GetValues();


    public void Clear(); 
}