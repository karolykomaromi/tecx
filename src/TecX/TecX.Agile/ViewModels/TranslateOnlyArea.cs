namespace TecX.Agile.ViewModels
{
    using System.Windows;
    using System.Windows.Input;

    using TecX.Common;

    public abstract class TranslateOnlyArea
    {
        public abstract Point GetMousePositionOnTranslateOnlyArea();
    }

    public class InputElementTranslateOnlyArea : TranslateOnlyArea
    {
        private readonly IInputElement element;

        public InputElementTranslateOnlyArea(IInputElement element)
        {
            Guard.AssertNotNull(element, "element");

            this.element = element;
        }

        public override Point GetMousePositionOnTranslateOnlyArea()
        {
            return Mouse.GetPosition(this.element);
        }
    }
}
