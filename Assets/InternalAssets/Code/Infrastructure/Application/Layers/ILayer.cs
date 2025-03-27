namespace ProjectOlog.Code.Infrastructure.Application.Layers
{
    /// <summary>
    /// Интерфейс, который предоставляет возможность LayersManager скрывать/показывать интерфейсы HUD или игры.
    /// </summary>
    public interface ILayer
    {
        void ShowLayer();

        void HideLayer();
    }
}