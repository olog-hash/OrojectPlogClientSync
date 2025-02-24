using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Entities.Objects.Instantiate
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct PostInstantiateObjectEvent : IComponent
    {
        public Entity ObjectEntity;
    }
}