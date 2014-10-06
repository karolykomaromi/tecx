namespace Infrastructure.UnityExtensions.Factories
{
    using System;
    using System.Reflection;

    public interface ITypedFactoryComponentSelector
    {
        TypedFactoryComponent SelectComponent(MethodInfo method, Type type, object[] arguments);
    }
}