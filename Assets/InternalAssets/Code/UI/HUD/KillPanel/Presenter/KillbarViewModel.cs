using ObservableCollections;
using ProjectOlog.Code.UI.Core;
using ProjectOlog.Code.UI.HUD.Killbar.Models;

namespace ProjectOlog.Code.UI.HUD.Killbar.Presenter
{
    public class KillbarViewModel : BaseViewModel
    {
        private const int MaxVisibleItems = 10;
        
        public ObservableList<KillbarItemModel> KillItems { get; } = new ObservableList<KillbarItemModel>();
        
        public void AddKillItem(KillbarItemModel item)
        {
            KillItems.Add(item);
            
            // Удаляем старые записи, если превышен лимит
            if (KillItems.Count > MaxVisibleItems)
            {
                var oldestItem = KillItems[0];
                KillItems.RemoveAt(0);
                oldestItem.Dispose();
            }
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var item in KillItems)
            {
                item.UpdateLifetime(deltaTime);
            }
        }
        
        public override void Dispose()
        {
            base.Dispose();

            foreach (var item in KillItems)
            {
                item.Dispose();
            }
            
            KillItems.Clear();
        }
    }
}