namespace TecX.Common.Test.Pipes
{
    using System.Collections.Generic;
    using System.Linq;

    using TecX.Common.Pipes;

    using Xunit;

    public class PipesFixture
    {
        [Fact]
        public void Should_Tunnel_Values_Through_Pipeline()
        {
            IEnumerable<int> numbers = new[] { 1, 2, 3 };

            Filter<int, string> pipeline = new Square().Pipe(new Printer());

            string[] text = pipeline.Process(numbers).ToArray();

            Assert.Equal("1", text[0]);
            Assert.Equal("4", text[1]);
            Assert.Equal("9", text[2]);
        }

        [Fact]
        public void Should_Use_Enumerable_To_Start_Pipeline()
        {
            var printer = new Printer();
            var pipeline = new Numbers(3).Pipe(new Square()).Pipe(printer);

            pipeline.Start();

            string[] text = printer.Text;

            Assert.Equal("1", text[0]);
            Assert.Equal("4", text[1]);
            Assert.Equal("9", text[2]);
        }
    }
}
