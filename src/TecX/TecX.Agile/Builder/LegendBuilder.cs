using TecX.Common;

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

        #region Methods

        public LegendBuilder With(string name, byte[] color)
        {
            Guard.AssertNotEmpty(name, "name");
            Guard.AssertNotNull(color, "color");

            ConstructedObject.Add(name, color);

            return this;
        }

        public LegendBuilder Without(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            ConstructedObject.Remove(name);

            return this;
        }

        public LegendBuilder Replace(string name, byte[] color)
        {
            Guard.AssertNotEmpty(name, "name");
            Guard.AssertNotNull(color, "color");

            ConstructedObject.Remove(name);
            ConstructedObject.Add(name, color);

            return this;
        }

        #endregion Methods

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

        #endregion Overrides of EntityBuilder<Legend>
    }
}