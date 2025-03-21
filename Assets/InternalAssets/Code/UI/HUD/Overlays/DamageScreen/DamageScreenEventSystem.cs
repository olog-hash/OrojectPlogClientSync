using ProjectOlog.Code.Features.Players.Core.Markers;
using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Mechanics.Mortality.Damage;
using ProjectOlog.Code.UI.HUD.Killbar.Presenter;
using ProjectOlog.Code.UI.HUD.Overlays.DamageScreen.Presenter;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.UI.HUD.Overlays.DamageScreen
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DamageScreenEventSystem : TickrateSystem
    {
        private Filter _damageEventFilter;
        private DamageScreenViewModel _damageScreenViewModel;

        public DamageScreenEventSystem(DamageScreenViewModel damageScreenViewModel)
        {
            _damageScreenViewModel = damageScreenViewModel;
        }

        public override void OnAwake()
        {
            _damageEventFilter = World.Filter.With<PostDamageEvent>().With<EntityVictimEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _damageEventFilter)
            {
                ref var postDamageEvent = ref entityEvent.GetComponent<PostDamageEvent>();
                ref var entityVictimEvent = ref entityEvent.GetComponent<EntityVictimEvent>();
                
                DamageEvent(entityVictimEvent.VictimEntity, postDamageEvent.ActualDamageCount);
            }
        }

        private void DamageEvent(Entity victimEntity, float damageCount)
        {
            if (victimEntity == null || victimEntity.IsNullOrDisposed()) return;
            if (!victimEntity.Has<LocalPlayerMarker>()) return;
            
            _damageScreenViewModel.ApplyDamage(damageCount);
        }
    }
}