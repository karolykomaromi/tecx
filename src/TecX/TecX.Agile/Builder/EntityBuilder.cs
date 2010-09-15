using System;

using TecX.Common;

namespace TecX.Agile.Builder
{
    /// <summary>
    /// Base class for implementations of the Builder Pattern
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to build</typeparam>
    public abstract class EntityBuilder<TEntity> : ICloneable
        where TEntity : ICloneable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the object under construction.
        /// </summary>
        protected TEntity ConstructedObject { get; set; }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBuilder&lt;TEntity&gt;"/> class.
        /// </summary>
        protected internal EntityBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBuilder&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="builder">A builder from which the <see cref="ConstructedObject"/> will be
        /// copied</param>
        protected EntityBuilder(EntityBuilder<TEntity> builder)
        {
            Guard.AssertNotNull(builder, "builder");

            ConstructedObject = (TEntity) builder.ConstructedObject.Clone();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBuilder{TEntity}"/> class.
        /// </summary>
        /// <param name="entity">The entity to build up</param>
        protected EntityBuilder(TEntity entity)
        {
            Guard.AssertNotNull(entity, "entity");

            ConstructedObject = (TEntity) entity.Clone();
        }

        #endregion c'tor

        #region Public Methods

        /// <summary>
        /// Builds an instance of the entity under construction.
        /// </summary>
        /// <returns>The built up entity</returns>
        public TEntity Build()
        {
            return (TEntity) ConstructedObject.Clone();
        }

        #endregion Public Methods

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public abstract object Clone();

        #endregion ICloneable Members

        #region Operators

        /// <summary>
        /// Performs an implicit conversion from <see cref="EntityBuilder{TEntity}"/> 
        /// to <typeparamref name="TEntity"/>
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TEntity(EntityBuilder<TEntity> builder)
        {
            return builder.Build();
        }

        #endregion Operators
    }
}