using System;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Mechanics.Repercussion.Damage.Core
{
    public class EntityLifeProcessor
    {
        private const int MAX_ARMOR = 100;
        
        public bool IsEntityAvaliable(Entity entity)
        {
            return !entity.IsNullOrDisposed() && entity.Has<HealthArmorComponent>();
        }
        
        public bool IsEntityNotAvaliableToDamage(Entity entity)
        {
            return !IsEntityAvaliable(entity) || entity.Has<DeadMarker>();
        }
        
        public bool DamageEntity(Entity entity, int damageAmount, out int realDamage)
        {
            realDamage = 0;

            if (IsEntityNotAvaliableToDamage(entity))
                return false;

            ref var healthArmor = ref entity.GetComponent<HealthArmorComponent>();

            if (healthArmor.HealthPointLeft <= 0) 
                return false;
            
            int totalHealthArmor = healthArmor.HealthPointLeft;
            realDamage = Math.Min(damageAmount, totalHealthArmor);

            if (realDamage >= totalHealthArmor)
            {
                healthArmor.Health = 0;
                healthArmor.Armor = 0;
            }
            else
            {
                if (healthArmor.Armor >= realDamage)
                {
                    healthArmor.Armor -= realDamage;
                }
                else
                {
                    int remainingDamage = realDamage - healthArmor.Armor;
                    healthArmor.Armor = 0;
                    healthArmor.Health -= remainingDamage;
                }
            }

            return true;
        }

        public void ReviveEntity(Entity entity)
        {
            if (!IsEntityAvaliable(entity)) return;
            
            if (entity.Has<DeadMarker>()) entity.RemoveComponent<DeadMarker>();
            
            ref var healthArmor = ref entity.GetComponent<HealthArmorComponent>();

            healthArmor.Health = healthArmor.MaxHealth;
            healthArmor.Armor = healthArmor.MaxArmor;
        }

        public bool ReplenishArmor(Entity entity, int replenishCount, out int realReplenish)
        {
            realReplenish = 0;

            if (IsEntityNotAvaliableToDamage(entity))
                return false;

            ref var healthArmor = ref entity.GetComponent<HealthArmorComponent>();

            if (replenishCount > 0)
            {
                if (healthArmor.Armor == MAX_ARMOR) 
                    return false;

                int availableSpace = MAX_ARMOR - healthArmor.Armor;
                realReplenish = Math.Min(replenishCount, availableSpace);
            }
            else
            {
                realReplenish = Math.Max(replenishCount, -healthArmor.Armor);
            }

            healthArmor.Armor += realReplenish;

            return true;
        }

        public bool ReplenishHealth(Entity entity, int replenishCount, out int realReplenish)
        {
            realReplenish = 0;

            if (IsEntityNotAvaliableToDamage(entity))
                return false;

            ref var healthArmor = ref entity.GetComponent<HealthArmorComponent>();

            if (replenishCount > 0)
            {
                if (healthArmor.Health == healthArmor.MaxHealth) 
                    return false;

                int availableSpace = healthArmor.MaxHealth - healthArmor.Health;
                realReplenish = Math.Min(replenishCount, availableSpace);
            }
            else
            {
                realReplenish = Math.Max(replenishCount, -healthArmor.Health);
            }

            healthArmor.Health += realReplenish;

            return true;
        }
        
        public bool IsShouldDie(Entity entity)
        {
            if (IsEntityNotAvaliableToDamage(entity))
                return false;
            
            ref var healthArmor = ref entity.GetComponent<HealthArmorComponent>();

            return healthArmor.HealthPointLeft <= 0;
        }

        public bool IsDead(Entity entity)
        {
            return IsEntityAvaliable(entity) && entity.Has<DeadMarker>();
        }
    }
}