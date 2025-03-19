namespace ProjectOlog.Code.UI.HUD.Killbar.Elements
{
    // Базовый класс для всех элементов полоски
    public abstract class KillbarElement
    {
        // Общие свойства, которые могут быть у любого элемента
        public float MarginLeft { get; set; } = 0f;
        public float MarginRight { get; set; } = 0f;
    }
}