using System.Windows;

namespace TecX.Agile.View.Behavior
{
    public class StyleUseableBehaviorCollection : FreezableCollection<System.Windows.Interactivity.Behavior>
    {
        protected override Freezable CreateInstanceCore()
        {
            return new StyleUseableBehaviorCollection();
        }
    }
}
