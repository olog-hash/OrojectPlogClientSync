using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.Debugger.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DebuggerLogicSystem : UpdateSystem
    {
        private DebuggerViewModel _debuggerViewModel;

        public DebuggerLogicSystem(DebuggerViewModel debuggerViewModel)
        {
            _debuggerViewModel = debuggerViewModel;
        }

        public override void OnAwake()
        {
            
        }

        public override void OnUpdate(float deltaTime)
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F3))
            {
                if (_debuggerViewModel.IsActive)
                {
                    _debuggerViewModel.OnHide();
                }
                else
                {
                    _debuggerViewModel.OnShow();
                }
            }
        }
    }
}