using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Prism.Commands;

namespace TecX.Agile.Infrastructure
{
    public static class Commands
    {
        public static readonly CompositeCommand HighlightField = new CompositeCommand();
    }

    public class CommandProxy
    {
        public CompositeCommand HighlightField
        {
            get { return Commands.HighlightField; }
        }
    }
}
