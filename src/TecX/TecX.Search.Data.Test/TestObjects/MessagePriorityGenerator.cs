namespace TecX.Search.Data.Test.TestObjects
{
    using System.Reflection;

    using Ploeh.AutoFixture.Kernel;

    public class MessagePriorityGenerator : ISpecimenBuilder
    {
        private int priority = -1;

        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;

            if (pi != null && pi.Name == "Priority")
            {
                this.priority = (this.priority + 1) % 5;

                return this.priority;
            }

            return new NoSpecimen(request);
        }
    }
}