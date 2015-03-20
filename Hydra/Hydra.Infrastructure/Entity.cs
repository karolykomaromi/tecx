namespace Hydra.Infrastructure
{
    using System;

    public abstract class Entity
    {
        public virtual long Id { get; set; }

        public virtual DateTime? DeletedAt { get; set; }

        public virtual string DeletedBy { get; set; }
    }
}