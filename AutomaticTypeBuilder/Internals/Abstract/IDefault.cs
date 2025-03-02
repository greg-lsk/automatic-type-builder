using System.Collections.ObjectModel;

namespace AutomaticTypeBuilder.Internals.Abstract;


internal interface IDefault
{
    public ReadOnlyDictionary<Type, Delegate> AssignmentLogic {get;} 
}