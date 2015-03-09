namespace Hydra.Hosting
{
    using System.Diagnostics.Contracts;

    public abstract class EmbeddedVisitor
    {
        public virtual void Visit(EmbeddedDirectory directory)
        {
            Contract.Requires(directory != null);
        }

        public virtual void Visit(EmbeddedFile file)
        {
            Contract.Requires(file != null);
        }
    }
}