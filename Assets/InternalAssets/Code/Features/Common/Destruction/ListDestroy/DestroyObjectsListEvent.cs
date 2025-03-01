using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Entities.Destruction.ListDestroy
{
    /// <summary>
    /// Событие для массового уничтожения игровых объектов.
    /// Содержит массив ID объектов, которые необходимо уничтожить.
    /// </summary>
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct DestroyObjectsListEvent : IComponent
    {
        public ushort[] DestructibleObjectIds;
    }
}