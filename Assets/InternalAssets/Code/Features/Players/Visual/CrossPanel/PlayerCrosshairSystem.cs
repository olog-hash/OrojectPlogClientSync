using System;
using ProjectOlog.Code._InDevs.Data;
using ProjectOlog.Code.Engine.Characters.KinematicCharacter.FirstPersonController;
using ProjectOlog.Code.UI.HUD.CrossHair.CrossPanel;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Features.Players.Visual.CrossPanel
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerCrosshairSystem : UpdateSystem
    {
        private LocalPlayerMonitoring _localPlayerMonitoring;
        private CrossViewModel _crossViewModel;

        public PlayerCrosshairSystem(LocalPlayerMonitoring localPlayerMonitoring, CrossViewModel crossViewModel)
        {
            _localPlayerMonitoring = localPlayerMonitoring;
            _crossViewModel = crossViewModel;
        }

        public override void OnAwake()
        {

        }

        public override void OnUpdate(float deltaTime)
        {
            // Базовые проверки
            if (_localPlayerMonitoring.IsDead() || !_localPlayerMonitoring.TryGetLocalPlayer(out Entity playerEntity))
            {
                _crossViewModel.IsTargetOnAim = false;
                return;
            }
    
            if (playerEntity.TryGetComponent(out ShotOrigin shotOrigin))
            {
                RaycastHit[] hits = Physics.RaycastAll(
                    shotOrigin.ShotOriginTranslation.position,
                    shotOrigin.ShotOriginTranslation.Transform.forward,
                    Mathf.Infinity
                );
                
                // Сортируем попадания по дистанции
                Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        
                if (hits.Length > 0)
                {
                    // Проверяем первый коллайдер
                    if (hits[0].collider.CompareTag("Player"))
                    {
                        // Если это игрок и есть второй коллайдер - проверяем его
                        if (hits.Length > 1)
                        {
                            _crossViewModel.IsTargetOnAim = hits[1].collider.GetComponent<DamagableMarker>() != null;
                        }
                        else
                        {
                            _crossViewModel.IsTargetOnAim = false;
                        }
                    }
                    else
                    {
                        // Если первый коллайдер не игрок - проверяем только его
                        _crossViewModel.IsTargetOnAim = hits[0].collider.GetComponent<DamagableMarker>() != null;
                    }
                }
                else
                {
                    _crossViewModel.IsTargetOnAim = false;
                }
            }
        }
    }
}