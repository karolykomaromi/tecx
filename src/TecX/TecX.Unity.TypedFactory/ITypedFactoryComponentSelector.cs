using System;
using System.Reflection;

namespace TecX.Unity.TypedFactory
{
    public interface ITypedFactoryComponentSelector
    {
        TypedFactoryComponent SelectComponent(MethodInfo method, Type type, object[] arguments);
    }
}
