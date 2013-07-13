namespace TecX.Unity.Configuration.Utilities
{
    using TecX.Common;
    using TecX.Unity.Utility;

    public class CompositeFilter<T>
    {
        private readonly CompositePredicate<T> excludes;
        private readonly CompositePredicate<T> includes;

        public CompositeFilter()
        {
            this.excludes = new CompositePredicate<T>();
            this.includes = new CompositePredicate<T>();
        }

        public CompositePredicate<T> Includes
        {
            get { return this.includes; }
            set { }
        }

        public CompositePredicate<T> Excludes
        {
            get { return this.excludes; }
            set { }
        }

        public bool Matches(T target)
        {
            return this.Includes.MatchesAny(target) && this.Excludes.DoesNotMatchAny(target);
        }
    }
}
