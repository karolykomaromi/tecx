namespace Infrastructure.Client.Test
{
    using System.Linq;
    using System.Windows.Controls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class VisualTreeFixture
    {
        [TestMethod]
        public void Should_Find_Parent_Of_Specified_Type()
        {
            TextBlock tb = new TextBlock();

            Grid grid = new Grid();
            grid.Children.Add(tb);

            UserControl expected = new UserControl { Content = grid };

            UserControl actual = VisualTree.Ancestor<UserControl>(tb);

            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void Should_Find_All_Parents_Of_Specified_Type()
        {
            TextBlock tb = new TextBlock();

            UserControl first = new UserControl { Content = tb };

            Grid grid = new Grid();

            grid.Children.Add(first);

            UserControl second = new UserControl { Content = grid };

            UserControl third = new UserControl { Content = second };

            var expected = new[] { first, second, third };

            var actual = VisualTree.Ancestors<UserControl>(tb).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
