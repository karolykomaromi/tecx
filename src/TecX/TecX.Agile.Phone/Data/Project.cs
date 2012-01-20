namespace TecX.Agile.Phone.Data
{
    using System.Globalization;
    using System.Threading;

    public class Project
    {
        private static int counter = 0;

        private readonly int id;

        public Project()
        {
            Interlocked.Increment(ref counter);

            this.id = counter * 1000;
        }

        public override string  ToString()
        {
            return this.id.ToString(CultureInfo.InvariantCulture);
        }
    }
}