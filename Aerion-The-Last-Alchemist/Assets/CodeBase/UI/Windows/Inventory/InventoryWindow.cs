using System.Collections.Generic;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Inventory
{
    public class InventoryWindow : WindowBase
    {
        [SerializeField] protected GameObject content;
        [SerializeField] protected GameObject cellprefab;
        [SerializeField] protected Button addAllButton;
        protected Dictionary<string, CellItem> _items = new Dictionary<string, CellItem>();
        
       protected void RefreshItem(Loot loot)
        {
            _items[loot.name]?.Show();
            _items[loot.name]?.SetItemCount(loot.value);
        }

        protected override void SubscribeUpdates()
        {
            Progress.gameData.lootData.Changed += RefreshItem;
            _onClose += () => {  Progress.gameData.lootData.Changed -= RefreshItem;};
        }
    }
}