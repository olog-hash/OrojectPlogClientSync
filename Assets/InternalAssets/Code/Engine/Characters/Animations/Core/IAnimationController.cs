using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger;

namespace ProjectOlog.Code.Engine.Characters.Animations.Core
{
    public interface IAnimationController
    {
        void UpdateAnimation(CharacterBodyLogger bodyLogger);
    }
}