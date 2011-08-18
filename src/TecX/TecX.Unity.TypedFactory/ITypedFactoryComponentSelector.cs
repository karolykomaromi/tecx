using System;
using System.Reflection;
using System.Text;

namespace TecX.Unity.TypedFactory
{
    public interface ITypedFactoryComponentSelector
    {
        TypedFactoryComponent SelectComponent(MethodInfo method, Type type, object[] arguments);
    }
}
