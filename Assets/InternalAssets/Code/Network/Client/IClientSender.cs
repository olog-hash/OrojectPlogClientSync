using LiteNetLib;
using LiteNetLib.Utils;

namespace ProjectOlog.Code.Network.Client
{
    public interface IClientSender
    {
        public NetDataWriter WritePacket(byte[] dataArray);

        public void Send(NetDataWriter writer, DeliveryMethod deliveryMethod);
    }
}