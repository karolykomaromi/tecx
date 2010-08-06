using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

using TecX.Common;
using TecX.Common.Comparison;
using TecX.Common.Extensions.Error;

namespace TecX.Agile
{
    /// <summary>
    /// Project data-object
    /// </summary>
    [Serializable]
    public class Project : PlanningArtefact, IEquatable<Project>, IEnumerable<Iteration>
    {
        #region Constants

        public static class Constants
        {
            /// <summary>Project</summary>
            public const string ProjectElementName = "Project";

            public static class PropertyNames
            {
                /// <summary>Iterations</summary>
                public const string IterationsPropertyName = "Iterations";

                /// <summary>Backlog</summary>
                public const string BacklogPropertyName = "Backlog";

                /// <summary>Legend</summary>
                public const string LegendPropertyName = "Legend";
            }
        }

        #endregion Constants

        ////////////////////////////////////////////////////////////

        #region Fields

        private readonly Dictionary<Guid, Iteration> _iterations;
        private Legend _legend;
        private Backlog _backlog;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        #region Properties

        /// <summary>
        /// Gets the iterations in this Project
        /// </summary>
        public IEnumerable<Iteration> Iterations
        {
            get { return _iterations.Values; }
        }

        /// <summary>
        /// Gets and sets the backlog of this Project
        /// </summary>
        public Backlog Backlog
        {
            get { return _backlog; }
            set
            {
                Guard.AssertNotNull(value, "value");

                _backlog = value;
            }
        }

        /// <summary>
        /// Gets the mappings between tasks and displayed colors
        /// </summary>
        public Legend Legend
        {
            get { return _legend; }
            set
            {
                Guard.AssertNotNull(value, "value");

                _legend = value;
            }
        }

        #endregion Properties

        ////////////////////////////////////////////////////////////

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        public Project()
        {
            _backlog = new Backlog();
            _legend = new Legend();
            _iterations = new Dictionary<Guid, Iteration>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        /// <param name="original">The original.</param>
        private Project(Project original)
            : this()
        {
            Guard.AssertNotNull(original, "original");

            CopyValuesFrom(original);
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Object Members

        public bool Equals(Project other)
        {
            Guard.AssertNotNull(other, "other");

            bool equal = base.Equals(other);
            equal &= Backlog.Equals(other.Backlog);
            equal &= Legend.Equals(other.Legend);
            equal &= Compare.AreEqual(_iterations, other._iterations,
                new DictionaryComparer<Guid, Iteration>());

            return equal;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Iteration> GetEnumerator()
        {
            return _iterations.Values.GetEnumerator();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            var other = obj as Project;

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

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion Object Members

        ////////////////////////////////////////////////////////////

        #region Methods

        /// <summary>
        /// Copies the values of all properties from another project. This does not include copying the iterations
        /// or the backlog inside the project or changing the Parent-field of backlog or iterations inside this project.
        /// In case you want a deep copy of this project use <see cref="Clone"/> instead.
        /// </summary>
        /// <remarks>Update does include the legend of the project</remarks>
        /// <param name="other">The project with the new values</param>
        private void CopyValuesFrom(Project other)
        {
            Guard.AssertNotNull(other, "other");

            base.CopyValuesFrom(other);

            Backlog.CopyValuesFrom(other.Backlog);

            _iterations.Clear();

            foreach (Iteration iteration in other.Iterations)
            {
                Add(iteration);
            }

            Legend.CopyValueFrom(other.Legend);
        }

        public void Add(Iteration iteration)
        {
            Guard.AssertNotNull(iteration, "iteration");

            if (_iterations.ContainsKey(iteration.Id))
                throw new ArgumentException("An iteration with the same Id already exists", "iteration")
                    .WithAdditionalInfo("existing", _iterations[iteration.Id]);

            _iterations.Add(iteration.Id, iteration);
        }

        public bool Remove(Guid id)
        {
            return _iterations.Remove(id);
        }

        #endregion Methods

        ////////////////////////////////////////////////////////////

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            var clone = new Project(this);
            return clone;
        }

        #endregion ICloneable Members
    }
}