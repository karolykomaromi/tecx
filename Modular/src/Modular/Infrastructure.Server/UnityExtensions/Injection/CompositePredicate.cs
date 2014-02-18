namespace Infrastructure.UnityExtensions.Injection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class CompositePredicate<T>
    {
        private readonly HashSet<Predicate<T>> predicates = new HashSet<Predicate<T>>();

        public CompositePredicate<T> Add(Predicate<T> filter)
        {
            Contract.Requires(filter != null);

            this.predicates.Add(filter);

            return this;
        }

        public bool MatchesNone(T target)
        {
            return !this.predicates.Any(p => p(target));
        }
    }
}