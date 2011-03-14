using System;

namespace TecX.Unity.Configuration.Expressions
{
    using System.Collections.Generic;

    using TecX.Common;

    public class OpenGenericFamilyExpression
    {
        #region Fields

        private readonly Type _from;
        private readonly List<Action<RegistrationFamily>> _alterations;
        private readonly List<Action<RegistrationGraph>> _children;

        #endregion Fields

        #region c'tor

        public OpenGenericFamilyExpression(Type from, Registry registry)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(registry, "registry");

            _from = from;

            _alterations = new List<Action<RegistrationFamily>>();
            _children = new List<Action<RegistrationGraph>>();

            registry.AddExpression(graph =>
                {
                    RegistrationFamily family = graph.FindFamily(_from);

                    _children.ForEach(action => action(graph));
                    _alterations.ForEach(action => action(family));
                });
        }

        #endregion c'tor
    }
}