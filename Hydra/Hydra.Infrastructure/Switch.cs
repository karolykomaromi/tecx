namespace Hydra.Infrastructure
{
    public class Switch
    {
        private readonly object obj;

        public Switch(object obj)
        {
            this.obj = obj;
        }

        public object Object
        {
            get { return this.obj; }
        }
    }
}
