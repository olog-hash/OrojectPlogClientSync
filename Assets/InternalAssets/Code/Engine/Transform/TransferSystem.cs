using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Transform
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TransferSystem : TickrateSystem
    {
        private Filter filter;

        public override void OnAwake()
        {
            filter = World.Filter.With<Interpolation.Interpolation>().With<Transfer>().Build();

            
        }

        public override void OnUpdate(float deltaTime) 
        {
            foreach (var entity in filter)
            {
                ref var interpolation = ref entity.GetComponent<Interpolation.Interpolation>();
                var transfer = entity.GetComponent<Transfer>();

                interpolation.CurrentTransform.Position += transfer.LinearVelocity * deltaTime;
                interpolation.CurrentTransform.Rotation *= Quaternion.Euler(transfer.AngularVelocity * deltaTime);
            }
        }
    }
}