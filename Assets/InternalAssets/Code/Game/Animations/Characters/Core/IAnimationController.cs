using ProjectOlog.Code.Game.Characters.KinematicCharacter.Logger;

namespace ProjectOlog.Code.Game.Animations.Characters.Core
{
    public interface IAnimationController
    {
        void UpdateAnimation(CharacterBodyLogger bodyLogger);
    }
}