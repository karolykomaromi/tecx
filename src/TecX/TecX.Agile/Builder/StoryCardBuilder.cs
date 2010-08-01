using System;

using TecX.Common;

namespace TecX.Agile.Builder
{
    /// <summary>
    /// Can be used to build an entity via a fluent interface
    /// </summary>
    public class StoryCardBuilder : PlanningArtefactBuilder<StoryCardBuilder, StoryCard>
    {
        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryCardBuilder"/> class.
        /// </summary>
        public StoryCardBuilder()
        {
            ConstructedObject = new StoryCard();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryCardBuilder"/> class. by copying another
        /// </summary>
        /// <param name="builder">The builder to copy</param>
        private StoryCardBuilder(EntityBuilder<StoryCard> builder)
            : base(builder)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryCardBuilder"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public StoryCardBuilder(StoryCard entity)
            : base(entity)
        {
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Public Methods

        /// <summary>
        /// Sets the priority
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public StoryCardBuilder WithPriority(Priority priority)
        {
            ConstructedObject.Tracking.Priority = priority;

            return this;
        }

        /// <summary>
        /// Sets the current side up.
        /// </summary>
        /// <param name="currentSideUp">The current side up.</param>
        /// <returns>Current instance of the builder. Fluent interface.</returns>
        public StoryCardBuilder WithCurrentSideUp(StoryCardSides currentSideUp)
        {
            ConstructedObject.CurrentSideUp = currentSideUp;

            return this;
        }

        /// <summary>
        /// Sets the description handwriting image.
        /// </summary>
        /// <param name="descriptionHandwritingImage">The description handwriting image.</param>
        /// <returns>Current instance of the builder. Fluent interface.</returns>
        public StoryCardBuilder WithDescriptionHandwritingImage(byte[] descriptionHandwritingImage)
        {
            Guard.AssertNotNull(descriptionHandwritingImage, "descriptionHandwritingImage");

            ConstructedObject.DescriptionHandwritingImage = descriptionHandwritingImage;

            return this;
        }

        /// <summary>
        /// Sets the owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <returns>Current instance of the builder. Fluent interface.</returns>
        public StoryCardBuilder WithTaskOwner(string owner)
        {
            Guard.AssertNotNull(owner, "owner");

            ConstructedObject.TaskOwner = owner;

            return this;
        }

        /// <summary>
        /// Sets the actual effort.
        /// </summary>
        /// <param name="actualEffort">The actual effort.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public StoryCardBuilder WithActualEffort(decimal actualEffort)
        {
            ConstructedObject.Tracking.ActualEffort = actualEffort;

            return this;
        }

        /// <summary>
        /// Sets the most likely estimate.
        /// </summary>
        /// <param name="mostLikelyEstimate">The most likely estimate.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public StoryCardBuilder WithMostLikelyEstimate(decimal mostLikelyEstimate)
        {
            ConstructedObject.Tracking.MostLikelyEstimate = mostLikelyEstimate;

            return this;
        }

        /// <summary>
        /// Sets the best case estimate.
        /// </summary>
        /// <param name="bestCaseEstimate">The best case estimate.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public StoryCardBuilder WithBestCaseEstimate(decimal bestCaseEstimate)
        {
            ConstructedObject.Tracking.BestCaseEstimate = bestCaseEstimate;

            return this;
        }

        /// <summary>
        /// Sets the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public StoryCardBuilder WithColor(byte[] color)
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
        public StoryCardBuilder WithRotationAngle(double rotationAngle)
        {
            ConstructedObject.View.RotationAngle = rotationAngle;

            return this;
        }

        /// <summary>
        /// Sets the status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public StoryCardBuilder WithStatus(Status status)
        {
            ConstructedObject.Tracking.Status = status;

            return this;
        }

        /// <summary>
        /// Sets the width.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public StoryCardBuilder WithWidth(double width)
        {
            ConstructedObject.View.Width = width;

            return this;
        }

        /// <summary>
        /// Sets the height.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <returns>Current instance of the builder. Fluent interface.</returns>
        public StoryCardBuilder WithHeight(double height)
        {
            ConstructedObject.View.Height = height;

            return this;
        }

        /// <summary>
        /// Sets the worst case estimate.
        /// </summary>
        /// <param name="worstCaseEstimate">The worst case estimate.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public StoryCardBuilder WithWorstCaseEstimate(decimal worstCaseEstimate)
        {
            ConstructedObject.Tracking.WorstCaseEstimate = worstCaseEstimate;

            return this;
        }

        /// <summary>
        /// Sets the X.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public StoryCardBuilder WithX(double x)
        {
            ConstructedObject.View.X = x;

            return this;
        }

        /// <summary>
        /// Sets the Y.
        /// </summary>
        /// <param name="y">The y.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public StoryCardBuilder WithY(double y)
        {
            ConstructedObject.View.Y = y;

            return this;
        }

        #endregion Public Methods

        ////////////////////////////////////////////////////////////

        #region ICloneable Members

        /// <summary>
        /// Implements <see cref="ICloneable.Clone"/>
        /// </summary>
        /// <returns>Deep copy of the builder</returns>
        public override object Clone()
        {
            return new StoryCardBuilder(this);
        }

        #endregion ICloneable Members
    }
}