using System.Linq.Expressions;

namespace AutomaticTypeBuilder.Internals.Concrete;


public class Instantiation : IInstantiation
{
    public Instantiate<T> For<T>(IInstantiationData instantiationData)
    {
        var instanceFieldTypes = instantiationData.Types.ToArray();
        var instanceFieldValues = instantiationData.Values.ToArray();

        var instanceParam = Expression.Variable(typeof(T), "instance");

        var constructor = typeof(T).GetConstructor(instanceFieldTypes) 
                        ?? throw new InvalidOperationException($"Could not get constructor info for Type:{typeof(T).Name}");

        var construstorArgs = new Expression[instanceFieldTypes.Length];
        for(int i = 0; i < construstorArgs.Length ; ++i)
        {
            construstorArgs[i] = Expression.Constant(instanceFieldValues[i], instanceFieldTypes[i]);
        }
   
        var newExpression = Expression.New(constructor, construstorArgs);
        var assignInstance = Expression.Assign(instanceParam, newExpression);

        var block = Expression.Block([instanceParam], assignInstance, instanceParam);
        var compiledLambda = Expression.Lambda<Instantiate<T>>(block).Compile();

        return compiledLambda;                         
    }
}