using System;

using TecX.Common;

namespace TecX.Agile.Builder
{
    /// <summary>
    /// Abstract base for implementations of the Builder Pattern that produce <see cref="PlanningArtefact"/> DTOs
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class PlanningArtefactBuilder<TBuilder, TEntity> : EntityBuilder<TEntity>
        where TEntity : PlanningArtefact, ICloneable
        where TBuilder : EntityBuilder<TEntity>
    {
        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanningArtefactBuilder&lt;TBuilder, TEntity&gt;"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected PlanningArtefactBuilder(TEntity entity)
            : base(entity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanningArtefactBuilder{TBuilder,TEntity}"/> class.
        /// </summary>
        protected internal PlanningArtefactBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanningArtefactBuilder{TBuilder,TEntity}"/> class.
        /// </summary>
        protected PlanningArtefactBuilder(EntityBuilder<TEntity> builder)
            : base(builder)
        {
        }

        #endregion c'tor

        #region Construction Methods for PlanningArtefact

        /// <summary>
        /// Sets the description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public TBuilder WithDescription(string description)
        {
            Guard.AssertNotNull(description, "description");

            ConstructedObject.Description = description;

            return (this as TBuilder);
        }

        /// <summary>
        /// Sets the ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public TBuilder WithId(Guid id)
        {
            ConstructedObject.Id = id;

            return (this as TBuilder);
        }

        /// <summary>
        /// Sets the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public TBuilder WithName(string name)
        {
            Guard.AssertNotNull(name, "name");

            ConstructedObject.Name = name;

            return (this as TBuilder);
        }

        #endregion Construction Methods for PlanningArtefact
    }
}