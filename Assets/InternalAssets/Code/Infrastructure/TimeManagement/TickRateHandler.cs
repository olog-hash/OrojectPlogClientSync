using System;

namespace ProjectOlog.Code.Infrastructure.TimeManagement
{
    public class TickRateHandler
    {
        private Action _tickUpdate;
        private float _minTimeBetweenTiks = 0;
        private float _timer = 0;
        private float _nextTickTime;

        public TickRateHandler(Action tickUpdate, float minTime = 1f / 20f)
        {
            _tickUpdate = tickUpdate;
            _minTimeBetweenTiks = minTime;

            _timer = 0;
            _nextTickTime = 0;
        }
        
        public void Update(float deltaTime)
        {
            _timer += deltaTime;

            while (_timer >= _minTimeBetweenTiks)
            {
                _timer -= _minTimeBetweenTiks;

                _tickUpdate();
            }
        }
        
        /*
        public void Update(float deltaTime)
        {
            // Проверяем, пора ли выполнять следующий тик
            if (Time.time >= _nextTickTime)
            {
                _tickUpdate?.Invoke();
                // Вычисляем время следующего тика
                _nextTickTime += _minTimeBetweenTiks;

                // Предотвращаем накопление отставания при падении FPS
                if (_nextTickTime < Time.time)
                {
                    _nextTickTime = Time.time + _minTimeBetweenTiks;
                }
            }
        }*/
    }
}