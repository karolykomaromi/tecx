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