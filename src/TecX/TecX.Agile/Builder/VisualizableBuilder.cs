namespace TecX.Agile.Builder
{
    public class VisualizableBuilder : EntityBuilder<Visualizable>
    {
        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualizableBuilder"/> class.
        /// </summary>
        public VisualizableBuilder()
        {
            ConstructedObject = new Visualizable();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualizableBuilder"/> class. Copy constructor
        /// </summary>
        /// <param name="builder">The builder to copy</param>
        public VisualizableBuilder(EntityBuilder<Visualizable> builder)
            : base(builder)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualizableBuilder"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public VisualizableBuilder(Visualizable entity)
            : base(entity)
        {
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Overrides of EntityBuilder<Visualizable>

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            return new VisualizableBuilder(this);
        }

        #endregion Overrides of EntityBuilder<Visualizable>
    }
}