using System;

using TecX.Common;

namespace TecX.Agile.ViewModel
{
    [Serializable]
    public class StoryCard : PlanningArtefact, IHighlightable
    {
        #region Implementation of IHighlightable

        public event EventHandler<HighlightEventArgs> HighlightingChanged = delegate { };

        public void NotifyGotFocus(string controlName)
        {
            //TODO weberse this will be the place where we notify remote clients that 
            //some field on the ui was highlighted
        }

        public void Highlight(string controlName)
        {
            Guard.AssertNotEmpty(controlName, "controlName");

            HighlightingChanged(this, new HighlightEventArgs(controlName));
        }

        #endregion Implementation of IHighlightable

        public PlanningArtefactCollection<StoryCard> Parent { get; internal set; }

        
    }
}