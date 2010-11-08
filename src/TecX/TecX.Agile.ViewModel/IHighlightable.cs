using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TecX.Agile.ViewModel
{
    //will be used to bring input control into focus when someone types into them at a remote
    //location (e.g. change description of a storycard and all other users will see whats done)
    public interface IHighlightable
    {
        event EventHandler<HighlightEventArgs> Highlight;

        void NotifyFieldHighlighted(string fieldName);

        void HighlightField(string fieldName);
    }
}
