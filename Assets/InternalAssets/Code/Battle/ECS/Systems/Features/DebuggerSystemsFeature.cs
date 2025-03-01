using ProjectOlog.Code.UI.HUD.Debugger.Systems;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Battle.ECS.Systems.Features
{
    public sealed class DebuggerSystemsFeature: FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<DebuggerLogSystem>(_systemsGroup);
        }
    }
}