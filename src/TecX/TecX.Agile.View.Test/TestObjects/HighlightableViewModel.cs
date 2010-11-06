using System;

using TecX.Agile.ViewModel;

namespace TecX.Agile.View.Test.TestObjects
{
    public class HighlightableViewModel : IHighlightable
    {
        public string HighlightedControlName { get; set; }

        public void Highlight(string controlName)
        {
            if(HighlightingChanged != null)
            {
                HighlightingChanged(this, new HighlightEventArgs(controlName));
            }
        }
        public event EventHandler<HighlightEventArgs> HighlightingChanged;

        public void NotifyGotFocus(string controlName)
        {
            HighlightedControlName = controlName;
        }
    }
}