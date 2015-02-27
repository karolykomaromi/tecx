namespace Hydra.Unity
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.Unity;

    public static class WithInjectionMembers
    {
        public static IEnumerable<InjectionMember> None(Type type)
        {
            return new InjectionMember[0];
        }
    }
}