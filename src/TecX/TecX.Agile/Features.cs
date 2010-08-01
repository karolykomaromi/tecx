using System;

namespace TecX.Agile
{
    /// <summary>
    /// Base for reusable elements related to visual or tracking features
    /// </summary>
    [Serializable]
    public abstract class Features<TFeature> : ICloneable, IEquatable<TFeature>
        where TFeature : Features<TFeature>
    {
        #region Implementation of ICloneable

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public abstract object Clone();

        #endregion Implementation of ICloneable

        ////////////////////////////////////////////////////////////

        #region Implementation of IEquatable<T>

        public abstract bool Equals(TFeature other);

        #endregion Implementation of IEquatable<T>

    }
}