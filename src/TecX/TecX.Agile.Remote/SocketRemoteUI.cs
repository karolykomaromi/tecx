using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using TecX.Agile.Infrastructure.Events;

namespace TecX.Agile.Remote
{
    public class SocketRemoteUI : IRemoteUI
    {
        public SocketRemoteUI()
        {
            //TODO weberse 2010-12-21 can I hook up the socket initialization to the rootvisual loaded event?
        }

        public void Handle(PropertyUpdated message)
        {
            throw new NotImplementedException();
        }

        public void Handle(StoryCardRescheduled message)
        {
            throw new NotImplementedException();
        }

        public void Handle(StoryCardPostponed message)
        {
            throw new NotImplementedException();
        }

        public void Handle(FieldHighlighted message)
        {
            throw new NotImplementedException();
        }

        public void Handle(StoryCardMoved message)
        {
            throw new NotImplementedException();
        }

        public void Handle(CaretMoved message)
        {
            throw new NotImplementedException();
        }
    }
}
