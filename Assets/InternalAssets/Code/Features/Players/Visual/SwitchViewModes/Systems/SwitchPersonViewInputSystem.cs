using ProjectOlog.Code._InDevs.Data;
using ProjectOlog.Code.Engine.Inputs;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Players.Visual.SwitchViewModes.Systems
{
    /// <summary>
    /// Система, которая отвечает за то, чтобы игрок мог переключать виды от 1го, 2го и 3го лица нажатием клавишь. 
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SwitchPersonViewInputSystem : UpdateSystem
    {
        private LocalPlayerMonitoring _localPlayerMonitoring;

        public SwitchPersonViewInputSystem(LocalPlayerMonitoring localPlayerMonitoring)
        {
            _localPlayerMonitoring = localPlayerMonitoring;
        }

        public override void OnAwake()
        {

        }

        public override void OnUpdate(float deltaTime)
        {
            if (_localPlayerMonitoring.IsDead()) return;
            
            if (InputControls.GetKeyDown(KeyType.Alpha_1))
            {
                World.CreateTickEvent().AddComponentData(new SwitchPersonViewEvent
                {
                    ViewType = EPersonViewType.First
                });
            }
            if (InputControls.GetKeyDown(KeyType.Alpha_2))
            {
                World.CreateTickEvent().AddComponentData(new SwitchPersonViewEvent
                {
                    ViewType = EPersonViewType.Second
                });
            }
            if (InputControls.GetKeyDown(KeyType.Alpha_3))
            {
                World.CreateTickEvent().AddComponentData(new SwitchPersonViewEvent
                {
                    ViewType = EPersonViewType.Third
                });
            }
        }
    }
}