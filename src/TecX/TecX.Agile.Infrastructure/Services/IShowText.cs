using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TecX.Agile.Infrastructure.Services
{
    public interface IShowText
    {
        string Text { get; }

        void Show(string text);
    }
}
