namespace TecX.EnumClasses.Test.Client
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.EnumClasses.Test.Client.SortingService;

    public class Program
    {
        public static void Main(string[] args)
        {
            var sortMe = new[]
                {
                    new SerializeMe { Text = "1" }, 
                    new SerializeMe { Text = "3" }, 
                    new SerializeMe { Text = "2" }
                };

            SerializeMe[] sorted;
            using (var proxy = new SortingServiceClient())
            {
                sorted = proxy.Sort(sortMe, SortOrder.Descending);
            }

            Assert.AreEqual("3", sorted[0].Text);
            Assert.AreEqual("2", sorted[1].Text);
            Assert.AreEqual("1", sorted[2].Text);
        }
    }
}
