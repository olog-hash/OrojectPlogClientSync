using ProjectOlog.Code._InDevs.Data;
using ProjectOlog.Code.Engine.Inputs;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Players.Visual.SpectatorPersonSystem.SpectatorPerson.Switching
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SpectatorSwitchInputSystem : UpdateSystem
    {
        private LocalPlayerMonitoring _localPlayerMonitoring;

        public SpectatorSwitchInputSystem(LocalPlayerMonitoring localPlayerMonitoring)
        {
            _localPlayerMonitoring = localPlayerMonitoring;
        }
        
        public override void OnAwake()
        {

        }

        public override void OnUpdate(float deltaTime)
        {
            if (!_localPlayerMonitoring.IsDead()) return;
            
            if (InputControls.GetKeyDown(KeyType.AltFire))
            {
                World.CreateTickEvent().AddComponent<SpectatorSwitchRequestEvent>();
            }
        }
    }
}