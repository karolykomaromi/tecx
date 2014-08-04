namespace TecX.Unity.Test.TestObjects
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