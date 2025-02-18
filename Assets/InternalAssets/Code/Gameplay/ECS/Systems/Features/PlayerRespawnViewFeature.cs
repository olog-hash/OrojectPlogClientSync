using ProjectOlog.Code._InDevs.Players.RemoteSync;
using ProjectOlog.Code._InDevs.Players.RespawnKinematicController;
using ProjectOlog.Code._InDevs.Players.Visual.PanelInfo;
using ProjectOlog.Code._InDevs.Players.Visual.Ragdoll;
using ProjectOlog.Code.Game.Animations.Characters;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Gameplay.ECS.Systems.Features
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