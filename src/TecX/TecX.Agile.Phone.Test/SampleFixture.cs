namespace TecX.Agile.Phone.Test
{
    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class SampleFixture
    {
        [Test]
        public void MyFirstTest()
        {
            var mock = new Mock<IService>();

            mock.Setup(s => s.DoMagic()).Returns("1");

            var svc = mock.Object;

            string result = svc.DoMagic();

            Assert.AreEqual("1", result);
        }
    }

    public interface IService
    {
        string DoMagic();
    }
}
