namespace TecX.Common.Specifications
{
    using System.Collections.Generic;

    public static class Extensions
    {
        public static IEnumerable<T> FindAll<T>(this IEnumerable<T> repository, Specification<T> specification)
        {
            Guard.AssertNotNull(repository, "repository");
            Guard.AssertNotNull(specification, "specification");

            List<T> itemsThatSatisfyTheSpecification = new List<T>();

            foreach (T candidate in repository)
            {
                if (specification.IsSatisfiedBy(candidate))
                {
                    itemsThatSatisfyTheSpecification.Add(candidate);
                }
            }

            return itemsThatSatisfyTheSpecification;
        }
    }
}