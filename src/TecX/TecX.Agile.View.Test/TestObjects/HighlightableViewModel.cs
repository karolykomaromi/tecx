using System;

using TecX.Agile.ViewModel;

namespace TecX.Agile.View.Test.TestObjects
{
    public class HighlightableViewModel : IHighlightable
    {
        public string HighlightedControlName { get; set; }

        public void HighlightField(string fieldName)
        {
            if(Highlight != null)
            {
                Highlight(this, new HighlightEventArgs(fieldName));
            }
        }
        public event EventHandler<HighlightEventArgs> Highlight;

        public void NotifyFieldHighlighted(string fieldName)
        {
            HighlightedControlName = fieldName;
        }
    }
}