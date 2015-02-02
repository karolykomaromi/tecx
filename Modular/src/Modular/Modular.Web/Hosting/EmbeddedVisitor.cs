namespace Modular.Web.Hosting
{
    public abstract class EmbeddedVisitor
    {
        public virtual void Visit(EmbeddedDirectory directory)
        {
        }

        public virtual void Visit(EmbeddedFile file)
        {
        }
    }
}