namespace AutomaticTypeBuilder;


public delegate T Instantiate<T>();

public interface IInstantiation
{
    public Instantiate<T> For<T>(IInstantiationData instantiationData);
}