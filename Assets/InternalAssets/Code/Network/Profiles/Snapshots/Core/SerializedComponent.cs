using System.Buffers;

namespace ProjectOlog.Code.Network.Profiles.Snapshots.Core
{
    public struct SerializedComponent
    {
        private readonly ArrayPool<byte> _arrayPool;
    
        public byte[] Array { get; }
    
        public int Length { get; }

        public SerializedComponent(ArrayPool<byte> arrayPool, byte[] array, int length)
        {
            _arrayPool = arrayPool;
            Array = array;
            Length = length;
        }

        public void Dispose()
        {
            //_arrayPool.Return(Array);
        }
    }
}