using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Agile.View.Test.TestObjects;
using TecX.Agile.ViewModel;

namespace TecX.Agile.View.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class HighlightingFixture
    {
        [TestMethod]
        public void WhenTellingViewModelToHighlightTextBox_TextBoxIsFocused()
        {
            HighlightableViewModel highlightable = new HighlightableViewModel();

            TestUserControl ctrl = new TestUserControl(highlightable);

            highlightable.Highlight("Txt");

            Assert.IsTrue(ctrl.Txt.IsFocused);
        }

        [TestMethod]
        public void WhenFocussingTextBox_ViewModelIsNotified()
        {
            HighlightableViewModel highlightable = new HighlightableViewModel();

            TestUserControl ctrl = new TestUserControl(highlightable);

            ctrl.Txt.Focus();

            Assert.AreEqual("Txt", highlightable.HighlightedControlName);
        }
    }
}
