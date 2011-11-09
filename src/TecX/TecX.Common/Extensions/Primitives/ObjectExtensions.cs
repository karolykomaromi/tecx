namespace TecX.Common.Extensions.Primitives
{
    using System;

    /// <summary>
    /// Extension methods for <see cref="object"/>
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Performs a safe cast using the <c>as</c> operator
        /// </summary>
        /// <typeparam name="TDestination">The type to cast to</typeparam>
        /// <param name="obj">The object to cast.</param>
        /// <returns>The casted object</returns>
        public static TDestination As<TDestination>(this object obj)
            where TDestination : class
        {
            return obj as TDestination;
        }

        /// <summary>
        /// Casts an object to the specified type.
        /// </summary>
        /// <typeparam name="TDestination">The type to cast to.</typeparam>
        /// <param name="obj">The object to cast.</param>
        /// <returns>The casted object</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="obj"/> is <c>null</c></exception>
        /// <exception cref="InvalidCastException">When <paramref name="obj"/> cannot be cast to 
        /// <typeparamref name="TDestination"/></exception>
        public static TDestination To<TDestination>(this object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            return (TDestination)obj;
        }

        /// <summary>
        /// Gets a <c>null</c>-safe hash value.
        /// </summary>
        /// <param name="obj">The object to hash.</param>
        /// <returns>0 if the <paramref name="obj"/> is <c>null</c>; otherwise the value of <c>obj.GetHashCode()</c></returns>
        public static int GetNullSafeHashCode(this object obj)
        {
            int hash = (obj == null) ? 0 : obj.GetHashCode();

            return hash;
        }
    }
}