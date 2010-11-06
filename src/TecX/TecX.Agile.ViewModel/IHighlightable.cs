using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Common;

namespace TecX.Agile.ViewModel
{
    //will be used to bring input control into focus when someone types into them at a remote
    //location (e.g. change description of a storycard and all other users will see whats done)
    public interface IHighlightable
    {
        event EventHandler<HighlightEventArgs> HighlightingChanged;

        void NotifyGotFocus(string controlName);

        void Highlight(string controlName);
    }

    public class HighlightEventArgs : EventArgs
    {
        private readonly string _controlName;

        public string ControlName
        {
            get { return _controlName; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightEventArgs"/> class
        /// </summary>
        public HighlightEventArgs(string controlName)
        {
            Guard.AssertNotEmpty(controlName, "controlName");

            _controlName = controlName;
        }
    }
 }
