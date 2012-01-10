namespace TecX.Common.Specifications
{
    using System.Collections.Generic;

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
        /// <typeparam name="TCandidate">The type of object the specification works on. </typeparam>
        /// <returns>All candidates that satisfy the specification</returns>
        public static IEnumerable<TCandidate> FindAll<TCandidate>(
            this IEnumerable<TCandidate> repository,
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