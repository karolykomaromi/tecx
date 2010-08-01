namespace TecX.Agile.Builder
{
    public class LegendBuilder : EntityBuilder<Legend>
    {
        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendBuilder"/> class.
        /// </summary>
        public LegendBuilder()
        {
            ConstructedObject = new Legend();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendBuilder"/> class. Copy constructor
        /// </summary>
        /// <param name="builder">The builder to copy</param>
        private LegendBuilder(EntityBuilder<Legend> builder)
            : base(builder)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendBuilder"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public LegendBuilder(Legend entity)
            : base(entity)
        {
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Overrides of EntityBuilder<Legend>

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            return new LegendBuilder(this);
        }

        #endregion
    }
}