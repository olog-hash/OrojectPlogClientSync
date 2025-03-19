using R3;

namespace ProjectOlog.Code.UI.Core.Services
{
    /// <summary>
    /// Глобальное управление видимостью UI.
    /// </summary>
    public static class GlobalUIVisibility
    {
        public static ReactiveProperty<bool> IsVisible { get; } = new ReactiveProperty<bool>(true);

        public static void Reset() => IsVisible.Value = true;
        public static void Toggle() => IsVisible.Value = !IsVisible.Value;
        public static void SetVisibility(bool isVisible) => IsVisible.Value = isVisible;
    }
}