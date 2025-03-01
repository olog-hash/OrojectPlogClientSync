using System;
using LiteNetLib.Utils;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Objects.Interactables.Core.Events
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct RemoteObjectDataBroadcast : IComponent
    {
        public EntityProvider ObjectProvider;
        public NetDataPackage[] DataPackagesArray;
    }
}