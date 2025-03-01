using System;

namespace ProjectOlog.Code.Network.Client
{
    public interface IClientConnector
    {
        event Action OnConnected;
        event Action<string> OnDisconnected;

        void ConnectedResult(bool result, string reason);
    }
}