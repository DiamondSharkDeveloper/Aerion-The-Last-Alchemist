using System.Collections.Generic;
using CodeBase.Data;
using UnityEngine;

namespace CodeBase.UI.Windows.Inventory
{
    public class InventoryWindow : WindowBase
    {
        [SerializeField] protected GameObject content;
        [SerializeField] protected GameObject cellprefab;
        protected Dictionary<string, CellItem> _items = new Dictionary<string, CellItem>();
        
       protected void RefreshItem(Loot loot)
        {
            _items[loot.name]?.Show();
            _items[loot.name]?.SetItemCount(loot.value);
        }

        protected override void SubscribeUpdates()
        {
            Progress.gameData.lootData.Changed += RefreshItem;
        }
    }
}