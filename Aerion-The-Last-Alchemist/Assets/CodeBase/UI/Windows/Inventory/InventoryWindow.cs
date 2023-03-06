using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.UI.Windows.Inventory
{
    public class InventoryWindow : WindowBase
    {
        [SerializeField] private GameObject content;
        [SerializeField] private GameObject cellprefab;
        private Dictionary<string, CellItem> _items = new Dictionary<string, CellItem>();

        public void Initialize(List<IngredientStaticData> staticDatas)
        {
            for (int i = 0; i < staticDatas.Count; i++)
            {
                _items[staticDatas[i].name] = Instantiate(cellprefab).GetComponent<CellItem>();
                _items[staticDatas[i].name].transform.SetParent(content.transform,false);
                _items[staticDatas[i].name].SetItemSprite(staticDatas[i].lootIcon);
                if (Progress.gameData.lootData.lootPiecesInDataDictionary.Dictionary.ContainsKey(staticDatas[i].name))
                {
                    RefreshItem(Progress.gameData.lootData.lootPiecesInDataDictionary.Dictionary[staticDatas[i]?.name]);
                }
                else
                {
                    _items[staticDatas[i].name].Hide();
                }
            }
        }

        private void RefreshItem(Loot loot)
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