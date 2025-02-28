using Scellecs.Morpeh;

namespace ProjectOlog.Code.Gameplay.ECS.Systems.Features
{
    public sealed class RepercussionFeature : FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateFeature<DamageRepercussionFeature>(_systemsGroup);
        }
    }
}