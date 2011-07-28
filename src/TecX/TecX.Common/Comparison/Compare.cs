using System;
using System.Collections.Generic;
using System.Linq;

namespace TecX.Common.Comparison
{
    /// <summary>
    /// Helper class for comparing objects.
    /// </summary>
    public static class Compare
    {
        #region Typewise overloads

        /// <summary>
        /// Compares two byte arrays for value equality.
        /// </summary>
        /// <param name="first">First array of bytes</param>
        /// <param name="second">Second array of bytes</param>
        /// <returns><c>true</c> if both arrays contain identical values or both are <c>null</c>; <c>false</c> otherwise.</returns>
        public static bool AreEqual(byte[] first, byte[] second)
        {
            //one is null -> false
            if (IsExactlyOneNull(first, second))
            {
                return false;
            }

            //both are null -> true
            if (AreBothNull(first, second))
            {
                return true;
            }
            //different length -> false
            if (first.Length != second.Length)
            {
                return false;
            }

            return first.SequenceEqual(second);
        }

        /// <summary>
        /// Compares two objects using their Equals() method
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns><c>true</c> if the objects are equal, <c>false</c> otherwise.</returns>
        public static bool AreEqual(object first, object second)
        {
            //one object is null -> not equal
            if (IsExactlyOneNull(first, second))
                return false;

            //both objects are null -> equal
            if (AreBothNull(first, second))
                return true;

            //uses the instance Equals() method for comparison
            bool equal = first.Equals(second);

            return equal;
        }

        /// <summary>
        /// Compares two objects using an implementation of <see cref="IEqualityComparer{T}"/>
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> of the objects to compare</typeparam>
        /// <param name="first">First object</param>
        /// <param name="second">Second object</param>
        /// <param name="comparer">Implementation of <see cref="IEqualityComparer{T}"/> used to
        /// compare the <paramref name="first"/> and <paramref name="second"/> object</param>
        /// <returns><c>true</c> if the two objects are considered equal; <c>false</c> otherwise</returns>
        public static bool AreEqual<T>(T first, T second, IEqualityComparer<T> comparer)
        {
            Guard.AssertNotNull(comparer, "comparer");

            //one object is null -> not equal
            if (IsExactlyOneNull(first, second))
                return false;

            //both objects are null -> equal
            if (AreBothNull(first, second))
                return true;

            bool equal = comparer.Equals(first, second);

            return equal;
        }

        #endregion Typewise overloads

        #region null Checks

        /// <summary>
        /// Determines whether none of the objects is null
        /// </summary>
        /// <param name="first">The first object</param>
        /// <param name="second">The second object</param>
        /// <returns>
        /// 	<c>true</c> if none is <c>null</c>; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNoneNull(object first, object second)
        {
            return (first != null) && (second != null);
        }

        /// <summary>
        ///Determines wether both values are <c>null</c>
        /// </summary>
        /// <param name="first">The first obj.</param>
        /// <param name="second">The second obj.</param>
        /// <returns><c>true</c> if both objects are <c>null</c>; <c>false</c> otherwise</returns>
        public static bool AreBothNull(object first, object second)
        {
            return (first == null) && (second == null);
        }

        /// <summary>
        /// Determines whether one of the values (and only one) is <c>null</c>
        /// </summary>
        /// <param name="first">The first obj.</param>
        /// <param name="second">The second obj.</param>
        /// <returns>
        /// 	<c>true</c> if exactly one of the values is <c>null</c>; <c>false</c> otherwise
        /// </returns>
        public static bool IsExactlyOneNull(object first, object second)
        {
            bool oneNull = (first == null) ^ (second == null);

            return oneNull;
        }

        #endregion null Checks
    }
}