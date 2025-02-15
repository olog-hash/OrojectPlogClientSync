using System;
using ProjectOlog.Code.Gameplay.ECS.Rules.GameRules.FirstAppeared.Spawn;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Core.Extensions
{
    public static class Extensions
    {
        public static T With<T>(this T self, Action<T> set)
        {
            set.Invoke(self);
            return self;
        }
 
        public static T With<T>(this T self, Action<T> apply, Func<bool> when)
        {
            if (when())
                apply?.Invoke(self);
 
            return self;
        }
 
        public static T With<T>(this T self, Action<T> apply, bool when)
        {
            if (when)
                apply?.Invoke(self);
 
            return self;
        }
        
        //

        public static Entity AddSpawnRequestRule(this Entity entity)
        {
            entity.AddComponent<SpawnRequestRule>();
            return entity;
        }

        public static Entity InstallRuleEntity(this Entity entity) =>
            entity
                .AddSpawnRequestRule();
                //.With(x => x.Cooldown);
    }
}