using System;

using Microsoft.Practices.ObjectBuilder2;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBuildKeyMapping
    {
        #region Fields

        private readonly Predicate<IBindingContext, IBuilderContext> _matches;
        private readonly Type _mapTo;
        private readonly string _uniqueMappingName;

        #endregion Fields

        #region Properties

        public NamedTypeBuildKey BuildKey
        {
            get { return new NamedTypeBuildKey(_mapTo, _uniqueMappingName); }
        }

        #endregion Properties

        #region c'tor

        public ContextualBuildKeyMapping(Predicate<IBindingContext, IBuilderContext> matches, Type mapTo, string uniqueMappingName)
        {
            //guards
            if (matches == null) throw new ArgumentNullException("matches");
            if (mapTo == null) throw new ArgumentNullException("mapTo");
            if (uniqueMappingName == null) throw new ArgumentNullException("uniqueMappingName");

            _matches = matches;
            _mapTo = mapTo;
            _uniqueMappingName = uniqueMappingName;
        }

        #endregion c'tor

        public bool Matches(IBindingContext bindingContext, IBuilderContext builderContext)
        {
            //guards
            if (bindingContext == null) throw new ArgumentNullException("bindingContext");
            if (builderContext == null) throw new ArgumentNullException("builderContext");

            return _matches(bindingContext, builderContext);
        }
    }
}