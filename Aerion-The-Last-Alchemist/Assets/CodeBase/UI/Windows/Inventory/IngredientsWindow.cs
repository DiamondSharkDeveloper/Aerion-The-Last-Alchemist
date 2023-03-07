using System.Collections.Generic;
using CodeBase.StaticData;

namespace CodeBase.UI.Windows.Inventory
{
    public class IngredientsWindow : InventoryWindow
    {
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
    }
}