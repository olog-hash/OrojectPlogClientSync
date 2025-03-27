namespace ProjectOlog.Code.DataStorage.Core
{
    /// <summary>
    /// Интерфейс реализующий метод для очистки реализуемого класса.
    /// Очистка происходит в самом начале игры и после ее закрытия.
    /// </summary>
    public interface IProjectContainer
    {
        /// <summary>
        /// Метод для очистки данных проектной части контейнера
        /// </summary>
        public void Reset();
    }
}