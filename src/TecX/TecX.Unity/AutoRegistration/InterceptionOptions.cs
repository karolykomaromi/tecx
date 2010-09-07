using System;
using System.Collections.Generic;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

using TecX.Common;

namespace TecX.Unity.AutoRegistration
{
    public class InterceptionOptions
    {
        #region Properties

        public Type Type { get; private set; }
        public InjectionMember Interceptor { get; private set; }
        public IEnumerable<InterceptionBehaviorBase> Behaviors { get; private set; }

        public InjectionMember[] InjectionMembers
        {
            get
            {
                List<InjectionMember> behaviors = new List<InjectionMember> { Interceptor };

                behaviors.AddRange(Behaviors);

                return behaviors.ToArray();
            }
        }

        #endregion Properties

        #region c'tor

        public InterceptionOptions(Type type, InjectionMember interceptor,
                                   IEnumerable<InterceptionBehaviorBase> behaviors)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotNull(interceptor, "interceptor");
            Guard.AssertNotNull(behaviors, "behaviors");

            Type = type;
            Interceptor = interceptor;
            Behaviors = behaviors;
        }

        #endregion c'tor

    }
}