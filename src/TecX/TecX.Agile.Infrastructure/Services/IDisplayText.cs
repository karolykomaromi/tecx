using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TecX.Agile.Infrastructure.Services
{
    public interface IDisplayText
    {
        string Text { get; }

        void Show(string text);
    }
}
