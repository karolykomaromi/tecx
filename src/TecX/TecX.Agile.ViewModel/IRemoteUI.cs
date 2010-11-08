using System;

namespace TecX.Agile.ViewModel
{
    public interface IRemoteUI
    {
        void HighlightField(Guid artefactId, string fieldName);
    }
}