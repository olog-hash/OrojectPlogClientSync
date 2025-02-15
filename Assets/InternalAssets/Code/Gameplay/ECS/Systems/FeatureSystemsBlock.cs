using Scellecs.Morpeh;

namespace ProjectOlog.Code.Gameplay.ECS.Systems
{
    public abstract class FeatureSystemsBlock
    {
        public abstract void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory);
    }
}