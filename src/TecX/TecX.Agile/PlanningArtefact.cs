#region using

using System;

using TecX.Common;
using TecX.Common.Comparison;

#endregion

namespace TecX.Agile
{
    /// <summary>
    /// Abstract base class for planning-artefact data-objects
    /// </summary>
    [Serializable]
    public abstract class PlanningArtefact : ICloneable, IEquatable<PlanningArtefact>
    {
        #region Fields

        private Guid _id;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets and sets the unique ID of the artefact
        /// </summary>
        public Guid Id
        {
            get { return _id; }
            set
            {
                if (_id != Guid.Empty)
                    throw new InvalidOperationException(
                        "The Id of a PlanningArtefact can only be changed once after its creation");

                _id = value;
            }
        }

        /// <summary>
        /// Gets and sets the name of the artefact
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the description of the artefact
        /// </summary>
        public string Description { get; set; }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanningArtefact"/> class.
        /// </summary>
        protected PlanningArtefact()
        {
            Id = Guid.Empty;
            Name = string.Empty;
            Description = string.Empty;
        }

        #endregion c'tor

        #region Overrides of Object

        public override int GetHashCode()
        {
            int hash = Id.GetHashCode();

            return hash;
        }

        public bool Equals(PlanningArtefact other)
        {
            Guard.AssertNotNull(other, "other");

            bool equal = Id == other.Id;
            equal &= Compare.AreEqual(Description, other.Description);
            equal &= Compare.AreEqual(Name, other.Name);

            return equal;
        }

        public override bool Equals(object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            PlanningArtefact other = obj as PlanningArtefact;

            if (other != null)
                return Equals(other);

            return false;
        }

        #endregion Overrides of Object

        #region Methods

        /// <summary>
        /// Copies the values of all properties from another <see cref="PlanningArtefact"/>.
        /// </summary>
        /// <param name="other">The artefact with the new values</param>
        protected void CopyValuesFrom(PlanningArtefact other)
        {
            Guard.AssertNotNull(other, "other");

            Description = other.Description;
            Id = other.Id;
            Name = other.Name;
        }

        #endregion Methods

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public abstract object Clone();

        #endregion ICloneable Members
    }
}