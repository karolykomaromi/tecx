namespace Hydra.Unity.Decoration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.ObjectBuilder2;

    public class DecoratorRegistrationsCollection
    {
        private readonly IDictionary<NamedTypeBuildKey, ICollection<Type>> registrations;

        public DecoratorRegistrationsCollection()
        {
            this.registrations = new Dictionary<NamedTypeBuildKey, ICollection<Type>>();
        }

        public ICollection<Type> this[NamedTypeBuildKey key]
        {
            get
            {
                Contract.Requires(key != null);
                Contract.Ensures(Contract.Result<ICollection<Type>>() != null);

                ICollection<Type> decorators;
                if (!this.registrations.TryGetValue(key, out decorators))
                {
                    // weberse 2015-02-27 HashSet avoids duplicates so if we run a convention twice we won't add the same type twice
                    decorators = new HashSet<Type>();

                    this.registrations[key] = decorators;
                }

                return decorators;
            }
        }

        public bool IsDecorated(NamedTypeBuildKey key)
        {
            Contract.Requires(key != null);

            ICollection<Type> decorators;
            if (this.registrations.TryGetValue(key, out decorators))
            {
                return decorators.Count > 1;
            }

            return false;
        }
    }
}