namespace TecX.Agile.Builder
{
    public class TrackableBuilder : EntityBuilder<Trackable>
    {
        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackableBuilder"/> class.
        /// </summary>
        public TrackableBuilder()
        {
            ConstructedObject = new Trackable();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackableBuilder"/> class. Copy constructor
        /// </summary>
        /// <param name="builder">The builder to copy</param>
        private TrackableBuilder(EntityBuilder<Trackable> builder)
            : base(builder)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackableBuilder"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public TrackableBuilder(Trackable entity)
            : base(entity)
        {
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Methods

        /// <summary>
        /// Sets the priority
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public TrackableBuilder WithPriority(Priority priority)
        {
            ConstructedObject.Priority = priority;

            return this;
        }

        /// <summary>
        /// Sets the actual effort.
        /// </summary>
        /// <param name="actualEffort">The actual effort.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public TrackableBuilder WithActualEffort(decimal actualEffort)
        {
            ConstructedObject.ActualEffort = actualEffort;

            return this;
        }

        /// <summary>
        /// Sets the most likely estimate.
        /// </summary>
        /// <param name="mostLikelyEstimate">The most likely estimate.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public TrackableBuilder WithMostLikelyEstimate(decimal mostLikelyEstimate)
        {
            ConstructedObject.MostLikelyEstimate = mostLikelyEstimate;

            return this;
        }

        /// <summary>
        /// Sets the best case estimate.
        /// </summary>
        /// <param name="bestCaseEstimate">The best case estimate.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public TrackableBuilder WithBestCaseEstimate(decimal bestCaseEstimate)
        {
            ConstructedObject.BestCaseEstimate = bestCaseEstimate;

            return this;
        }

        /// <summary>
        /// Sets the status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public TrackableBuilder WithStatus(Status status)
        {
            ConstructedObject.Status = status;

            return this;
        }

        /// <summary>
        /// Sets the worst case estimate.
        /// </summary>
        /// <param name="worstCaseEstimate">The worst case estimate.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public TrackableBuilder WithWorstCaseEstimate(decimal worstCaseEstimate)
        {
            ConstructedObject.WorstCaseEstimate = worstCaseEstimate;

            return this;
        }

        #endregion Methods

        ////////////////////////////////////////////////////////////

        #region Overrides of EntityBuilder<Trackable>

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            return new TrackableBuilder(this);
        }

        #endregion
    }
}