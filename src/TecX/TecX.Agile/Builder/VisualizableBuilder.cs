using TecX.Common;

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

        #region Methods

        /// <summary>
        /// Sets the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public VisualizableBuilder WithColor(byte[] color)
        {
            Guard.AssertNotNull(color, "color");

            ConstructedObject.Color = (byte[])color.Clone();

            return this;
        }

        /// <summary>
        /// Sets the rotation angle.
        /// </summary>
        /// <param name="rotationAngle">The rotation angle.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public VisualizableBuilder WithRotationAngle(double rotationAngle)
        {
            ConstructedObject.RotationAngle = rotationAngle;

            return this;
        }

        /// <summary>
        /// Sets the width.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public VisualizableBuilder WithWidth(double width)
        {
            ConstructedObject.Width = width;

            return this;
        }

        /// <summary>
        /// Sets the height.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <returns>Current instance of the builder. Fluent interface.</returns>
        public VisualizableBuilder WithHeight(double height)
        {
            ConstructedObject.Height = height;

            return this;
        }

        /// <summary>
        /// Sets the X.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public VisualizableBuilder WithX(double x)
        {
            ConstructedObject.X = x;

            return this;
        }

        /// <summary>
        /// Sets the Y.
        /// </summary>
        /// <param name="y">The y.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public VisualizableBuilder WithY(double y)
        {
            ConstructedObject.Y = y;

            return this;
        }

        #endregion Methods

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