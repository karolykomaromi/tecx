using System;

namespace TecX.Agile.ViewModel.Remote
{
    public interface IRemoteUI
    {
        void HighlightField(Guid artefactId, string fieldName);

        void UpdateProperty(Guid artefactId, string propertyName, object newValue);
    }
}