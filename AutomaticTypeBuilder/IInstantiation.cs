namespace AutomaticTypeBuilder;


public delegate void Instantiate<T>(out T instance);

public interface IInstantiation
{
    public Instantiate<T> For<T>(IInstantiationData instantiationData);
}