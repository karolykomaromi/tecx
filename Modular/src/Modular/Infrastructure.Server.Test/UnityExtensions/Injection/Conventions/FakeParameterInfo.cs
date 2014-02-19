namespace Infrastructure.Server.Test.UnityExtensions.Injection.Conventions
{
    using System;
    using System.Reflection;

    public class FakeParameterInfo : ParameterInfo
    {
        public FakeParameterInfo(string name, Type parameterType)
        {
            this.NameImpl = name;
            this.ClassImpl = parameterType;
        }
    }
}