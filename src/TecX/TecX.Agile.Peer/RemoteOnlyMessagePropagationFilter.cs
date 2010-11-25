﻿using System.ServiceModel;
using System.ServiceModel.Channels;

namespace TecX.Agile.Peer
{
    public class RemoteOnlyMessagePropagationFilter : PeerMessagePropagationFilter
    {
        public RemoteOnlyMessagePropagationFilter()
        {

        }

        public override PeerMessagePropagation ShouldMessagePropagate(Message message, PeerMessageOrigination origination)
        {
            PeerMessagePropagation destination = PeerMessagePropagation.LocalAndRemote;

            if (origination == PeerMessageOrigination.Local)

                destination = PeerMessagePropagation.Remote;

            return destination;
        }
    }
}