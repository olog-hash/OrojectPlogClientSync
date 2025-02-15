using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.UI.HUD.KillPanel.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class KillbarLogicSystem : TickrateSystem
    {
        private KillBarViewModel _killbarViewModel;

        public KillbarLogicSystem(KillBarViewModel killbarViewModel)
        {
            _killbarViewModel = killbarViewModel;
        }

        public override void OnAwake()
        {

        }

        public override void OnUpdate(float deltaTime)
        {
            _killbarViewModel.OnUpdate(deltaTime);
        }
    }
}