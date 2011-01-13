using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TecX.Agile.Infrastructure.Services
{
    public interface IShowViewModels
    {
        void Show(object viewModel, string regionName);
    }
}
