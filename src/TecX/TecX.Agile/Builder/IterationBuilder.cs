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

        #region Public Methods

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
        /// Sets the priority
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithPriority(Priority priority)
        {
            ConstructedObject.Tracking.Priority = priority;

            return this;
        }

        /// <summary>
        /// Sets the actual effort.
        /// </summary>
        /// <param name="actualEffort">The actual effort.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithActualEffort(decimal actualEffort)
        {
            ConstructedObject.Tracking.ActualEffort = actualEffort;

            return this;
        }

        /// <summary>
        /// Sets the most likely estimate.
        /// </summary>
        /// <param name="mostLikelyEstimate">The most likely estimate.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithMostLikelyEstimate(decimal mostLikelyEstimate)
        {
            ConstructedObject.Tracking.MostLikelyEstimate = mostLikelyEstimate;

            return this;
        }

        /// <summary>
        /// Sets the best case estimate.
        /// </summary>
        /// <param name="bestCaseEstimate">The best case estimate.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithBestCaseEstimate(decimal bestCaseEstimate)
        {
            ConstructedObject.Tracking.BestCaseEstimate = bestCaseEstimate;

            return this;
        }

        /// <summary>
        /// Sets the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithColor(byte[] color)
        {
            Guard.AssertNotNull(color, "color");

            ConstructedObject.View.Color = (byte[]) color.Clone();

            return this;
        }

        /// <summary>
        /// Sets the rotation angle.
        /// </summary>
        /// <param name="rotationAngle">The rotation angle.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithRotationAngle(double rotationAngle)
        {
            ConstructedObject.View.RotationAngle = rotationAngle;

            return this;
        }

        /// <summary>
        /// Sets the status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithStatus(Status status)
        {
            ConstructedObject.Tracking.Status = status;

            return this;
        }

        /// <summary>
        /// Sets the width.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithWidth(double width)
        {
            ConstructedObject.View.Width = width;

            return this;
        }

        /// <summary>
        /// Sets the height.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <returns>Current instance of the builder. Fluent interface.</returns>
        public IterationBuilder WithHeight(double height)
        {
            ConstructedObject.View.Height = height;

            return this;
        }

        /// <summary>
        /// Sets the worst case estimate.
        /// </summary>
        /// <param name="worstCaseEstimate">The worst case estimate.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithWorstCaseEstimate(decimal worstCaseEstimate)
        {
            ConstructedObject.Tracking.WorstCaseEstimate = worstCaseEstimate;

            return this;
        }

        /// <summary>
        /// Sets the X.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithX(double x)
        {
            ConstructedObject.View.X = x;

            return this;
        }

        /// <summary>
        /// Sets the Y.
        /// </summary>
        /// <param name="y">The y.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public IterationBuilder WithY(double y)
        {
            ConstructedObject.View.Y = y;

            return this;
        }

        #endregion Public Methods
    }
}