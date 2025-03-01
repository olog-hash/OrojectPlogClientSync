using Scellecs.Morpeh;

namespace ProjectOlog.Code.Battle.ECS.Systems
{
    public abstract class FeatureSystemsBlock
    {
        public abstract void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory);
    }
}