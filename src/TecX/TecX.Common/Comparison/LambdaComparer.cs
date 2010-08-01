using System;
using System.Collections.Generic;

using TecX.Common.Extensions.Primitives;

namespace TecX.Common.Comparison
{
    /// <summary>
    /// An implementation of <see cref="IEqualityComparer{T}"/> that uses a lambda function to check for equality
    /// </summary>
    /// <typeparam name="T">The type of the objects to compare</typeparam>
    internal class LambdaComparer<T> : IEqualityComparer<T>
    {
        #region Fields

        private readonly Func<T, T, bool> _lambdaComparer;
        private readonly Func<T, int> _lambdaHash;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaComparer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="lambdaComparer">The lambda comparer.</param>
        public LambdaComparer(Func<T, T, bool> lambdaComparer) :
            this(lambdaComparer, o => o.GetNullSafeHashCode())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaComparer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="lambdaComparer">The function that compares the two values</param>
        /// <param name="lambdaHash">The function that gets a hash-value for the values</param>
        public LambdaComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
        {
            Guard.AssertNotNull(lambdaComparer, "lambdaComparer");
            Guard.AssertNotNull(lambdaHash, "lambdaHash");

            _lambdaComparer = lambdaComparer;
            _lambdaHash = lambdaHash;
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region IEqualityComparer Members

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <typeparamref name="T"/> to compare.</param>
        /// <param name="y">The second object of type <typeparamref name="T"/>/> to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(T x, T y)
        {
            return _lambdaComparer(x, y);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
        /// </exception>
        public int GetHashCode(T obj)
        {
            Guard.AssertNotNull(obj, "obj");

            return _lambdaHash(obj);
        }

        #endregion IEqualityComparer Members
    }
}