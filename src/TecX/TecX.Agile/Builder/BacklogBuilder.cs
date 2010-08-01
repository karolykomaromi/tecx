namespace TecX.Agile.Builder
{
    /// <summary>
    /// Builder for creating <see cref="Backlog"/> instances
    /// </summary>
    public class BacklogBuilder : StoryCardContainerBuilder<BacklogBuilder, Backlog>
    {
        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="BacklogBuilder"/> class.
        /// </summary>
        public BacklogBuilder()
        {
            ConstructedObject = new Backlog();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BacklogBuilder"/> class. Copy constructor.
        /// </summary>
        /// <param name="builder">The builder to copy</param>
        private BacklogBuilder(EntityBuilder<Backlog> builder)
            : base(builder)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BacklogBuilder"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public BacklogBuilder(Backlog entity)
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
            return new BacklogBuilder(this);
        }

        #endregion ICloneable Members
    }
}