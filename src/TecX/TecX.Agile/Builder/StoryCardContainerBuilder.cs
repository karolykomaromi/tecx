using System;

using TecX.Common;

namespace TecX.Agile.Builder
{
    /// <summary>
    /// Abstract base for implementations of the Builder Pattern that produce <see cref="StoryCardContainer"/> DTOs
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class StoryCardContainerBuilder<TBuilder, TEntity> : PlanningArtefactBuilder<TBuilder, TEntity>
        where TEntity : StoryCardContainer, ICloneable
        where TBuilder : EntityBuilder<TEntity>
    {
        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryCardContainerBuilder{TBuilder,TEntity}"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected StoryCardContainerBuilder(TEntity entity)
            : base(entity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryCardContainerBuilder{TBuilder,TEntity}"/> class.
        /// </summary>
        protected internal StoryCardContainerBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryCardContainerBuilder{TBuilder,TEntity}"/> class.
        /// </summary>
        protected StoryCardContainerBuilder(EntityBuilder<TEntity> builder)
            : base(builder)
        {
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Construction Methods for ExtendedIndexCard

        /// <summary>
        ///Adds a storycard.
        /// </summary>
        /// <param name="aStorycard">The story-card.</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public TBuilder With(StoryCard aStorycard)
        {
            Guard.AssertNotNull(aStorycard, "aStorycard");

            ConstructedObject.Add(aStorycard);

            return this as TBuilder;
        }

        /// <summary>
        ///Removes a story-card.
        /// </summary>
        /// <param name="id">The id of the story-card to remove</param>
        /// <returns>Current instance of the builder. Fluent interface</returns>
        public TBuilder Without(Guid id)
        {
            ConstructedObject.Remove(id);

            return this as TBuilder;
        }

        #endregion Construction Methods for ExtendedIndexCard
    }
}