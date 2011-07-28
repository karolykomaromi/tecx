using System.Collections.Generic;

namespace TecX.Common.Specifications
{
    /// <summary>
    /// Extension methods to use with the specification pattern
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Extension method. Finds all objects that satisfy the specification
        /// </summary>
        /// <param name="repository">The repository of candidates</param>
        /// <param name="specification">The specification.</param>
        /// <returns>All candidates that satisfy the specification</returns>
        public static IEnumerable<TCandidate> FindAll<TCandidate>(this IEnumerable<TCandidate> repository,
                                                                  ISpecification<TCandidate> specification)
        {
            Guard.AssertNotNull(repository, "repository");
            Guard.AssertNotNull(specification, "specification");

            List<TCandidate> itemsThatSatisfyTheSpecification = new List<TCandidate>();

            foreach (TCandidate candidate in repository)
            {
                if (specification.IsMatch(candidate, null))
                {
                    itemsThatSatisfyTheSpecification.Add(candidate);
                }
            }

            return itemsThatSatisfyTheSpecification;
        }
    }
}