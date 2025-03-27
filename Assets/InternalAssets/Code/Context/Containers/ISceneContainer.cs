namespace ProjectOlog.Code.DataStorage.Core
{
    /// <summary>
    /// Интерфейс реализующий метод для очистки реализуемого класса.
    /// Очистка происходит в самом начале игры и после выхода со сцены/комнаты/карты.
    /// </summary>
    public interface ISceneContainer
    {
        /// <summary>
        /// Метод для очистки данных сценической части (уровня) контейнера
        /// </summary>
        public void Reset();
    }
}