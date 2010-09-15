using TecX.Common;

namespace TecX.Agile.Builder
{
    /// <summary>
    /// Implements the Builder Pattern for <see cref="Project"/> objects
    /// </summary>
    public class ProjectBuilder : PlanningArtefactBuilder<ProjectBuilder, Project>
    {
        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectBuilder"/> class.
        /// </summary>
        public ProjectBuilder()
        {
            ConstructedObject = new Project();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectBuilder"/> class.
        /// </summary>
        /// <param name="builder">The builder from which to copy the entity under construction</param>
        private ProjectBuilder(EntityBuilder<Project> builder)
            : base(builder)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectBuilder"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public ProjectBuilder(Project entity)
            : base(entity)
        {
        }

        #endregion c'tor

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            return new ProjectBuilder(this);
        }

        #endregion ICloneable Members

        #region Public Methods

        /// <summary>
        /// Sets the legend.
        /// </summary>
        /// <param name="legend">The legend.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public ProjectBuilder WithLegend(LegendBuilder legend)
        {
            Guard.AssertNotNull(legend, "legend");

            ConstructedObject.Legend.CopyValueFrom(legend.Build());

            return this;
        }

        /// <summary>
        /// Sets the backlog.
        /// </summary>
        /// <param name="backlog">The backlog</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public ProjectBuilder With(BacklogBuilder backlog)
        {
            Guard.AssertNotNull(backlog, "backlog");

            ConstructedObject.Backlog.CopyValuesFrom(backlog.Build());

            return this;
        }

        /// <summary>
        /// Adds an iteration.
        /// </summary>
        /// <param name="iteration">The iteration.</param>
        /// <returns>Current instance of the builder. Fluent interface.</returns>
        public ProjectBuilder With(IterationBuilder iteration)
        {
            Guard.AssertNotNull(iteration, "iteration");

            ConstructedObject.Add(iteration.Build());

            return this;
        }

        /// <summary>
        /// Removes an iteration
        /// </summary>
        /// <param name="iteration">The iteration.</param>
        /// <returns>Current instance of the builder. Fluent interface.</returns>
        public ProjectBuilder Without(IterationBuilder iteration)
        {
            Guard.AssertNotNull(iteration, "iteration");

            ConstructedObject.Remove(iteration.Build().Id);

            return this;
        }

        #endregion Public Methods
    }
}