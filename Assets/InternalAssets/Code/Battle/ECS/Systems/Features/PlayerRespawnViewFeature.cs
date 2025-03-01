using ProjectOlog.Code.Features.Players.Respawn;
using ProjectOlog.Code.Features.Players.Visual.PanelInfo;
using ProjectOlog.Code.Features.Players.Visual.Ragdoll;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Battle.ECS.Systems.Features
{
    public sealed class PlayerRespawnViewFeature : FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            // Ragdolls
            _systemsFactory.CreateSystem<RespawnRagdollSystem>(_systemsGroup);
            
            _systemsFactory.CreateSystem<RespawnNicknameSystem>(_systemsGroup);

            _systemsFactory.CreateSystem<RespawnKinematicControllerSystem>(_systemsGroup);
        }
    }
}