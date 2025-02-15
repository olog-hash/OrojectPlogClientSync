using ProjectOlog.Code.Game.Characters.KinematicCharacter.Interpolation;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Logger;
using ProjectOlog.Code.Game.Core;
using ProjectOlog.Code.Input.PlayerInput.FirstPerson;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Game.Characters.KinematicCharacter.FirstPersonController
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FirstPersonCharacterMovementSystem : FixedUpdateSystem 
    {
        private Filter filter;

        public override void OnAwake()
        {
            filter = World.Filter
                .With<Translation>()
                .With<CharacterInterpolation>()
                .With<FirstPersonInputs>()
                .With<FirstPersonCharacter>()
                .With<KinematicCharacterBody>()
                .With<FirstCharacterStateMachine>()
                .With<CharacterBodyLogger>()
                .Without<DeadMarker>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            // Assign the global data of the processor
            FirstPersonCharacterProcessor processor = default;

            processor.DeltaTime = deltaTime;

            foreach (Entity entity in filter)
            {
                ref var translation = ref GetComponent<Translation>(entity);
                ref var characterInterpolation = ref GetComponent<CharacterInterpolation>(entity);
                ref var kinematicCharacterBody = ref GetComponent<KinematicCharacterBody>(entity);
                ref var firstPersonCharacter = ref GetComponent<FirstPersonCharacter>(entity);
                ref var cameraInterpolation = ref GetComponent<CharacterInterpolation>(firstPersonCharacter.CharacterViewEntity);
                ref var firstCharacterStateMachine = ref GetComponent<FirstCharacterStateMachine>(entity);
                ref var characterBodyLogger = ref GetComponent<CharacterBodyLogger>(entity);
                var transitionStateRequestComponent = entity.Has<TransitionStateRequestComponent>() ? GetComponent<TransitionStateRequestComponent>(entity) : new TransitionStateRequestComponent();
                var firstPersonCharacterInputs = GetComponent<FirstPersonInputs>(entity);

                processor.Entity = entity;
                processor.Translation = translation.position;
                processor.Rotation = translation.rotation;
                processor.CharacterBody = kinematicCharacterBody;
                processor.FirstPersonCharacter = firstPersonCharacter;
                processor.FirstPersonInputs = firstPersonCharacterInputs;
                processor.FirstCharacterStateMachine = firstCharacterStateMachine;
                processor.TransitionStateRequestComponent = transitionStateRequestComponent;
                processor.CharacterBodyLogger = characterBodyLogger;

                // Update character
                processor.OnUpdate();

                kinematicCharacterBody = processor.CharacterBody;
                firstPersonCharacter = processor.FirstPersonCharacter;
                firstCharacterStateMachine = processor.FirstCharacterStateMachine;
                characterBodyLogger = processor.CharacterBodyLogger;
                

                characterInterpolation.CurrentTransform = new RigidTransform(processor.Rotation, processor.Translation);
                cameraInterpolation.CurrentTransform.position = firstPersonCharacter.CameraPointHeight * Vector3.up;
                
                // Delete single-request component
                if (transitionStateRequestComponent.IsActive) entity.RemoveComponent<TransitionStateRequestComponent>();
            }
        }
    }
}