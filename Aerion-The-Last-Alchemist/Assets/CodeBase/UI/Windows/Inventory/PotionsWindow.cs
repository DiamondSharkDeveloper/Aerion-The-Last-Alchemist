using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.StaticData;

namespace CodeBase.UI.Windows.Inventory
{
    public class PotionsWindow : InventoryWindow
    {
        private  List<FormulaStaticData> _staticData = new List<FormulaStaticData>();
        public void Initialize(List<FormulaStaticData> staticData)
        {
            _staticData = staticData;
            for (int i = 0; i < staticData.Count; i++)
            {
                _items[staticData[i].name] = Instantiate(cellprefab).GetComponent<CellItem>();
                _items[staticData[i].name].transform.SetParent(content.transform,false);
                _items[staticData[i].name].SetItemSprite(staticData[i].sprite);
                if (Progress.gameData.lootData.lootPiecesInDataDictionary.Dictionary.ContainsKey(staticData[i].name))
                {
                    RefreshItem(Progress.gameData.lootData.lootPiecesInDataDictionary.Dictionary[staticData[i]?.name]);
                }
                else
                {
                    _items[staticData[i].name].Hide();
                }
                addAllButton?.onClick.AddListener(AddAll);
            }
        }
        protected void AddAll()
        {
            foreach (FormulaStaticData formulaStaticData in _staticData)
            {
                Progress.gameData.lootData.lootPiecesInDataDictionary.Dictionary[formulaStaticData.name]=new Loot(formulaStaticData.name,10);;
                RefreshItem(Progress.gameData.lootData.lootPiecesInDataDictionary.Dictionary[formulaStaticData.name]);
            }
        }
    }
}