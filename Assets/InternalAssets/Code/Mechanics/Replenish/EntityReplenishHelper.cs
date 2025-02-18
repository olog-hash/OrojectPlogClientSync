using System;
using ProjectOlog.Code.Mechanics.Mortality.Core;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Mechanics.Replenish
{
    public class EntityReplenishHelper : EntityMortalityHelper
    {
        private const int MAX_ARMOR = 100;
        
        public bool TryReplenishArmor(Entity entity, int replenishCount, out int realReplenish)
        {
            realReplenish = 0;

            if (!IsEntityNotNull(entity) || !IsEntityAlive(entity))
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

        public bool TryReplenishHealth(Entity entity, int replenishCount, out int realReplenish)
        {
            realReplenish = 0;

            if (!IsEntityNotNull(entity) || !IsEntityAlive(entity))
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
    }
}