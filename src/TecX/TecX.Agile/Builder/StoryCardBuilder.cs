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
        /// Sets the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>Current instance of the builder. Fluent interface.</returns>
        public StoryCardBuilder WithView(VisualizableBuilder view)
        {
            Guard.AssertNotNull(view, "view");

            ConstructedObject.View = view;

            return this;
        }

        /// <summary>
        /// Sets the tracking information
        /// </summary>
        /// <param name="tracking">The tracking information</param>
        /// <returns>Current instance of the builder. Fluent interface.</returns>
        public StoryCardBuilder WithTracking(TrackableBuilder tracking)
        {
            Guard.AssertNotNull(tracking, "tracking");

            ConstructedObject.Tracking = tracking;

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