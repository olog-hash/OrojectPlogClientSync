using LiteNetLib;
using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Client
{
    public interface IClientSender
    {
        public NetDataWriter WritePacket(byte[] dataArray);

        public void Send(NetDataWriter writer, DeliveryMethod deliveryMethod);
    }
}