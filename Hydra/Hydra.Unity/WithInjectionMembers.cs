namespace Hydra.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.Unity;

    public static class WithInjectionMembers
    {
        public static IEnumerable<InjectionMember> None(Type type)
        {
            Contract.Ensures(Contract.Result<IEnumerable<InjectionMember>>() != null);

            return new InjectionMember[0];
        }
    }
}