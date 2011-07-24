using System;
using System.Collections.Generic;

using TecX.Common.Extensions.Primitives;

namespace TecX.Common.Comparison
{
    /// <summary>
    /// An implementation of <see cref="IEqualityComparer{T}"/> that uses a lambda function to check for equality
    /// </summary>
    /// <typeparam name="T">The type of the objects to compare</typeparam>
    public class LambdaEqualityComparer<T> : EqualityComparer<T>
    {
        private readonly Func<T, T, bool> _equals;
        private readonly Func<T, int> _hash;

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="equals">The lambda comparer.</param>
        public LambdaEqualityComparer(Func<T, T, bool> equals) :
            this(equals, o => o.GetNullSafeHashCode())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="equals">The function that compares the two values</param>
        /// <param name="hash">The function that gets a hash-value for the values</param>
        public LambdaEqualityComparer(Func<T, T, bool> equals, Func<T, int> hash)
        {
            Guard.AssertNotNull(equals, "equals");
            Guard.AssertNotNull(hash, "hash");

            _equals = equals;
            _hash = hash;
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <typeparamref name="T"/> to compare.</param>
        /// <param name="y">The second object of type <typeparamref name="T"/>/> to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public override bool Equals(T x, T y)
        {
            return _equals(x, y);
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
        public override int GetHashCode(T obj)
        {
            Guard.AssertNotNull(obj, "obj");

            return _hash(obj);
        }
    }
}