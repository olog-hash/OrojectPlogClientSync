namespace ProjectOlog.Code.UI.HUD.Killbar.Elements
{
    // Элемент-изображение
    // Элемент-изображение
    public class ImageElement : KillbarElement
    {
        public string ImagePath { get; set; }
        public float Width { get; set; } = 61f;
        public float Height { get; set; } = 15f; // В оригинале 15px
        public bool FlexGrow { get; set; } = true; // По умолчанию как в оригинале
    }
}