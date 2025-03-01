namespace ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger
{
    /// <summary>
    /// Представляет возможные состояния тела персонажа
    /// </summary>
    public enum ECharacterBodyState : byte
    {
        None = 0,
        Idle,     // Стоит на месте
        Walk,     // Обычная ходьба
        Crouch,   // Присел
        Sprint,   // Бег
        Jump,     // Прыжок
        NoClip,   // Режим прохождения сквозь стены
    }
}