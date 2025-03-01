using ProjectOlog.Code.Engine.Characters.KinematicCharacter.FirstPersonController.States;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Characters.KinematicCharacter.FirstPersonController
{
	[System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct FirstCharacterStateMachine : IComponent 
	{
        //[HideInInspector]
        public CharacterState CurrentCharacterState;
        //[HideInInspector]
        public CharacterState PreviousCharacterState;

        [HideInInspector]
        public GroundMoveState GroundMoveState;
        [HideInInspector]
        public FlyingNoCollisionsState FlyingNoCollisionsState;

        public static FirstCharacterStateMachine GetDefault()
        {
            var sm = new FirstCharacterStateMachine()
            {
                CurrentCharacterState = CharacterState.GroundMove,
                PreviousCharacterState = CharacterState.GroundMove,

                GroundMoveState = new GroundMoveState(),
                FlyingNoCollisionsState = new FlyingNoCollisionsState(),
            };

            return sm;
        }

        public void OnStateEnter(CharacterState state, CharacterState previousState, ref FirstPersonCharacterProcessor processor)
        {
            switch (state)
            {
                case CharacterState.GroundMove:
                    GroundMoveState.OnStateEnter(previousState, ref processor);
                    break;
                case CharacterState.FlyingNoCollisions:
                    FlyingNoCollisionsState.OnStateEnter(previousState, ref processor);
                    break;
            }
        }

        public void OnStateExit(CharacterState state, CharacterState newState, ref FirstPersonCharacterProcessor processor)
        {
            switch (state)
            {
                case CharacterState.GroundMove:
                    GroundMoveState.OnStateExit(newState, ref processor);
                    break;
                case CharacterState.FlyingNoCollisions:
                    FlyingNoCollisionsState.OnStateExit(newState, ref processor);
                    break;
            }
        }

        public void OnStateUpdate(CharacterState state, ref FirstPersonCharacterProcessor processor)
        {
            switch (state)
            {
                case CharacterState.GroundMove:
                    GroundMoveState.OnStateUpdate(ref processor);
                    break;
                case CharacterState.FlyingNoCollisions:
                    FlyingNoCollisionsState.OnStateUpdate(ref processor);
                    break;
            }
        }
    }
}