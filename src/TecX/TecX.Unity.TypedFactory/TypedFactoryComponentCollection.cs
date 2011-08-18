using System;
using System.Collections.Generic;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.TypedFactory
{
    public class TypedFactoryComponentCollection : TypedFactoryComponent
    {
        private readonly Type _collectionType;

        private readonly Delegate _addObjectToTypedList;

        public TypedFactoryComponentCollection(Type collectionItemType, Type collectionType, ParameterOverrides additionalArguments)
            : base(null, collectionItemType, additionalArguments)
        {
            Guard.AssertNotNull(collectionType, "collectionType");

            _collectionType = collectionType;

            _addObjectToTypedList = ReflectionHelper.CastItemAndAddToList(ComponentType);
        }

        public override object Resolve(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            IEnumerable<object> resolved = container.ResolveAll(ComponentType, AdditionalArguments);

            object theList = ReflectionHelper.CreateGenericListOf(ComponentType);

            foreach (object item in resolved)
            {
                _addObjectToTypedList.DynamicInvoke(item, theList);
            }

            if (_collectionType.IsArray)
            {
                object theArray = ReflectionHelper.ToArray(ComponentType, theList);
                return theArray;
            }

            return theList;
        }
    }
}