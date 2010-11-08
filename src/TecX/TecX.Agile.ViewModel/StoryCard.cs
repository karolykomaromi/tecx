using System;

using TecX.Common;

namespace TecX.Agile.ViewModel
{
    [Serializable]
    public class StoryCard : PlanningArtefact, IHighlightable
    {
        #region Implementation of IHighlightable

        public event EventHandler<HighlightEventArgs> Highlight = delegate { };

        public void NotifyFieldHighlighted(string fieldName)
        {
            //TODO weberse this will be the place where we notify remote clients that 
            //some field on the ui was highlighted
            if (Parent != null &&
                Parent.Project != null)
            {
                Parent.Project.NotifyFieldHighlighted(Id, fieldName);
            }
        }

        public void HighlightField(string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "controlName");

            Highlight(this, new HighlightEventArgs(fieldName));
        }

        #endregion Implementation of IHighlightable

        public PlanningArtefactCollection<StoryCard> Parent { get; internal set; }

    }
}