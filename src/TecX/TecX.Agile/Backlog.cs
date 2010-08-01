using System;

using TecX.Common;

namespace TecX.Agile
{
    /// <summary>
    /// Data-object for a Project backlog
    /// </summary>
    [Serializable]
    public class Backlog : StoryCardContainer, IEquatable<Backlog>
    {
        #region Constants

        public static class Constants
        {
            /// <summary>Backlog</summary>
            public const string BacklogName = "Backlog";
        }

        #endregion Constants

        ////////////////////////////////////////////////////////////

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="Backlog"/> class.
        /// </summary>
        public Backlog()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Backlog"/> class.
        /// </summary>
        /// <param name="original">The original backlog.</param>
        private Backlog(Backlog original)
            : this()
        {
            Guard.AssertNotNull(original, "original");

            CopyValuesFrom(original);
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Overrides of PlanningArtefact

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override object Clone()
        {
            var clone = new Backlog(this);

            return clone;
        }

        #endregion Overrides of PlanningArtefact

        ////////////////////////////////////////////////////////////

        #region Implementation of IEquatable<Backlog>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Backlog other)
        {
            Guard.AssertNotNull(other, "other");

            bool equal = base.Equals(other);

            return equal;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            Backlog other = obj as Backlog;

            if (other != null)
                return Equals(other);

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            //method solely exists to get rid of
            // 'overrides Equals() but does not override GetHashCode()' warning 
            return base.GetHashCode();
        }

        #endregion
    }
}