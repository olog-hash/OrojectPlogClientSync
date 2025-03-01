using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Network.Client
{
    /// <summary>Synchronizes server time to clients.</summary>
    public static class NetworkTime
    {
        // Константы
        public const int DEFAULT_TICK_RATE = 20;

        // События
        public static Action OnTickRateWasChanged = null;
        
        // === Параметры тикрейта ===
        
        /// <summary>
        /// Текущее значение Tickrate (стандартное 20).
        /// </summary>
        public static int TickRate { get; private set; } = DEFAULT_TICK_RATE;
        
        /// <summary>
        /// Временной интервал между тиками (1 / tickrate)
        /// </summary>
        public static float TickInterval => 1f / TickRate;

        /// <summary>
        /// Последний TickRate с сервера (на данный момент)
        /// </summary>
        public static uint LastServerTick { get; private set; }

        /// <summary>
        /// Последний локальный тикрейт (как правило для клиента)
        /// </summary>
        public static uint LastLocalTick { get; private set; }
        
        /// <summary>
        /// Значение TickRate (интервала) на самого последнего тикрейта.
        /// </summary>
        public static float LastTickUpdateTimeInterval { get; private set; }

        /// <summary>
        /// Значение Time.time на момент последнего тикрейта.
        /// </summary>
        public static double LastTickUpdateElapsedTime { get; private set; }

        public static int LastPing { get; private set; }

        /// <summary>Returns double precision clock time _in this system_, unaffected by the network.</summary>
        public static double localTime
        {
            // NetworkTime uses unscaled time and ignores Time.timeScale.
            // fixes Time.timeScale getting server & client time out of sync:
            // https://github.com/MirrorNetworking/Mirror/issues/3409
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Time.unscaledTimeAsDouble;
        }
        
        // === Методы ===

        /// <summary>
        /// Вызывать в самом начале игры (на ивент подписан RuntimeHelper)
        /// </summary>
        public static void Reset()
        {
            OnTickRateWasChanged = null;
            
            ClearPreviousData();
        }
        
        /// <summary>
        /// Стирает все старые значение тикрейта (полезно при новом подключении)
        /// </summary>
        public static void ClearPreviousData()
        {
            LastLocalTick = 0;
            LastServerTick = 0;
            LastTickUpdateTimeInterval = 0;
            LastTickUpdateElapsedTime = 0;
            LastPing = 0;
        }
        
        /// <summary>
        /// Позволяет вручную установить новое значение тикрейта
        /// </summary>
        public static void SetTickrate(int newTickrate)
        {
            TickRate = newTickrate;
            OnTickRateWasChanged?.Invoke();
            
            Debug.Log($"Was set {TickRate} ticks per second");
        }
        
        /// <summary>
        /// Обновляет значение LastServerTick на 1 единицу 
        /// </summary>
        public static void UpdateServerTick()
        {
            LastServerTick++;
        }
        
        public static void SetServerTick(uint newServerTick)
        {
            LastServerTick = newServerTick;
        }
        
        /// <summary>
        /// Обновляет значение LastLocalTick на 1 единицу (+ снимает временные мерки с других временных данных) 
        /// </summary>
        public static void UpdateLocalTick()
        {
            LastLocalTick++;
            LastTickUpdateTimeInterval = TickInterval;
            LastTickUpdateElapsedTime = Time.time;
        }

        public static void UpdatePing(int newPing)
        {
            LastPing = newPing;
        }
    }
}