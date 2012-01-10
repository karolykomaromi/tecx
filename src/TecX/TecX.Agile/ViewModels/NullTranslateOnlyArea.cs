namespace TecX.Agile.ViewModels
{
    using System.Windows;

    public class NullTranslateOnlyArea : TranslateOnlyArea
    {
        public override Point GetMousePositionOnTranslateOnlyArea()
        {
            return new Point();
        }
    }
}