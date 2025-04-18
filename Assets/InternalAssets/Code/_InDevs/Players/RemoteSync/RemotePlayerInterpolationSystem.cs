﻿using ProjectOlog.Code.Game.Characters.KinematicCharacter.Logger;
using ProjectOlog.Code.Game.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.Players.RemoteSync
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RemotePlayerInterpolationSystem : UpdateSystem  // Изменить на UpdateSystem вместо TickrateSystem
    {
        private Filter _remotePlayerFilter;

        public override void OnAwake()
        {
            _remotePlayerFilter = World.Filter
                .With<Translation>()
                .With<MirrorInterpolationComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _remotePlayerFilter)
            {
                ref var mirrorInterpolation = ref entity.GetComponent<MirrorInterpolationComponent>();
                ref var characterBodyLogger =  ref entity.GetComponent<CharacterBodyLogger>();
            
                // Обновление позиции должно происходить в Update цикле
                mirrorInterpolation.MirrorInterpolation.UpdatePosition(deltaTime, ref characterBodyLogger);
            }
        }
    }
}