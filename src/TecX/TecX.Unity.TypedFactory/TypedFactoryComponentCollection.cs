using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.TypedFactory
{
    public class TypedFactoryComponentCollection : TypedFactoryComponent
    {
        private readonly Type _collectionType;

        private readonly Delegate _addItemsToList;

        public TypedFactoryComponentCollection(Type collectionItemType, Type collectionType, ParameterOverrides additionalArguments)
            : base(null, collectionItemType, additionalArguments)
        {
            Guard.AssertNotNull(collectionType, "collectionType");

            _collectionType = collectionType;

            _addItemsToList = ReflectionHelper.CastItemAndAddToList(ComponentType);
        }

        public override object Resolve(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            IEnumerable<object> resolved = container.ResolveAll(ComponentType, AdditionalArguments);

            object theList = ReflectionHelper.CreateGenericListOf(ComponentType);

            foreach(object item in resolved)
            {
                _addItemsToList.DynamicInvoke(item, theList);
            }

            if(_collectionType.IsArray)
            {
                //TODO weberse 2011-08-18 convert to array and return
                throw new NotImplementedException();
            }

            return theList;
        }
    }
}