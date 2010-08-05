using System;

using TecX.Common;

namespace TecX.Agile.Builder
{
    /// <summary>
    /// Builder for creating <see cref="Iteration"/> instances
    /// </summary>
    public class IterationBuilder : StoryCardContainerBuilder<IterationBuilder, Iteration>
    {
        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="IterationBuilder"/> class.
        /// </summary>
        public IterationBuilder()
        {
            ConstructedObject = new Iteration();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IterationBuilder"/> class. Copy constructor
        /// </summary>
        /// <param name="builder">The builder to copy</param>
        private IterationBuilder(EntityBuilder<Iteration> builder)
            : base(builder)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IterationBuilder"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public IterationBuilder(Iteration entity)
            : base(entity)
        {
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region ICloneable Members

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A deep copy of this instance</returns>
        public override object Clone()
        {
            return new IterationBuilder(this);
        }

        #endregion ICloneable Members

        ////////////////////////////////////////////////////////////

        #region Methods

        /// <summary>
        /// Sets the end date.
        /// </summary>
        /// <param name="endDate">The end date.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithEndDate(DateTime endDate)
        {
            ConstructedObject.EndDate = endDate;

            return this;
        }

        /// <summary>
        /// Sets the available effort.
        /// </summary>
        /// <param name="availableEffort">The available effort.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithAvailableEffort(decimal availableEffort)
        {
            ConstructedObject.AvailableEffort = availableEffort;

            return this;
        }

        /// <summary>
        /// Sets the start date.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithStartDate(DateTime startDate)
        {
            ConstructedObject.StartDate = startDate;

            return this;
        }

        /// <summary>
        /// Sets the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithView(VisualizableBuilder view)
        {
            Guard.AssertNotNull(view, "view");

            ConstructedObject.View = view;

            return this;
        }

        /// <summary>
        /// Sets the tracking information.
        /// </summary>
        /// <param name="tracking">The tracking.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithTracking(TrackableBuilder tracking)
        {
            Guard.AssertNotNull(tracking, "tracking");

            ConstructedObject.Tracking = tracking;

            return this;
        }

        #endregion Methods
    }
}