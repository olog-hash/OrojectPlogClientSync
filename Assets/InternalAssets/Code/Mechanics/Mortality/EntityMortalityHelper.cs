using System;
using ProjectOlog.Code.Features.Entities.ShieldProtect;
using ProjectOlog.Code.Mechanics.Mortality.Core;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Mechanics.Mortality
{
    public class EntityMortalityHelper
    {
        /// <summary>
        /// Сущность подходит для взаимодействия
        /// Это ключевая проверка необходимая вначале ЛЮБОГО взаимодействия со здоровьем сущности.
        /// </summary>
        /// <returns></returns>
        public bool IsEntityNotNull(Entity entity)
        {
            return entity != null && !entity.IsNullOrDisposed() && entity.Has<HealthArmorComponent>();
        }

        /// <summary>
        /// Сущность на данный момент жива
        /// </summary>
        /// <returns></returns>
        protected bool IsEntityAlive(Entity entity)
        {
            return !entity.Has<DeadMarker>();
        }

        /// <summary>
        /// Сущность на данный момент неуязвима
        /// </summary>
        /// <returns></returns>
        protected bool IsEntityInvisible(Entity entity)
        {
            return entity.Has<ShieldProtectComponent>();
        }

        /// <summary>
        /// Наносим урон сущности
        /// </summary>
        /// <param name="expectedDamage">поступаемый урон</param>
        /// <returns></returns>
        public bool TryMakeDamage(Entity entity, int expectedDamage)
        {
            int actualDamage = 0;

            if (!IsEntityNotNull(entity))
                return false;

            if (expectedDamage <= 0)
                return false;

            ref var healthArmor = ref entity.GetComponent<HealthArmorComponent>();

            if (healthArmor.HealthPointLeft <= 0)
                return false;

            // Реальный урон не может быть выше максимального здоровья и брони
            actualDamage = Math.Min(expectedDamage, healthArmor.HealthPointLeft);

            // Сначала снимаем броню
            int armorDamage = Math.Min(actualDamage, healthArmor.Armor);
            healthArmor.Armor -= armorDamage;

            // Оставшийся урон идёт по здоровью
            int healthDamage = actualDamage - armorDamage;
            healthArmor.Health -= healthDamage;

            return true;
        }

        /// <summary>
        /// Пытаемся возродить сущность (если она мертва)
        /// </summary>
        /// <returns></returns>
        public bool TryReborn(Entity entity)
        {
            if (!IsEntityNotNull(entity))
                return false;

            if (IsEntityAlive(entity))
                return false;

            ref var healthArmor = ref entity.GetComponent<HealthArmorComponent>();

            healthArmor.Health = healthArmor.MaxHealth;
            healthArmor.Armor = healthArmor.MaxArmor;

            // Убираем маркер смерти
            entity.RemoveComponent<DeadMarker>();

            return true;
        }

        /// <summary>
        /// Пытаемся убить сущность
        /// </summary>
        /// <returns></returns>
        public bool TryKill(Entity entity)
        {
            if (!IsEntityNotNull(entity))
                return false;

            ref var healthArmor = ref entity.GetComponent<HealthArmorComponent>();

            healthArmor.Health = 0;
            healthArmor.Armor = 0;

            // Добавляем маркер смерти
            entity.AddComponent<DeadMarker>();

            return true;
        }
    }
}