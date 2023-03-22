using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Inventory
{
    public class IngredientsWindow : WindowBase
    {
        private readonly List<string> _ingredientsKeys = new List<string>();
        private readonly List<string> _formulaKeys = new List<string>();
        [SerializeField] private GameObject content;
        [SerializeField] private GameObject cellprefab;
        [SerializeField] private Button potionsButton;
        [SerializeField] private Button ingredientsButton;

        [SerializeField] private Sprite activeSprite;
        [SerializeField] private Sprite anActiveSprite;

        [SerializeField] private Button addAllPotionsButton;
        private Dictionary<string, CellItem> _items = new Dictionary<string, CellItem>();
        private bool _isPotionTab;

        private void RefreshItem(Loot loot)
        {
            _items[loot.name]?.Show();
            _items[loot.name]?.SetItemCount(loot.value);
        }

        protected override void SubscribeUpdates()
        {
            Progress.gameData.lootData.Changed += RefreshItem;
        }

        public void Initialize(List<IngredientStaticData> ingredientStaticData,
            List<FormulaStaticData> formulaStaticData)
        {
            _isPotionTab = false;
            CreateIngredients(ingredientStaticData);
            CreatePotions(formulaStaticData);
            HideCells(_formulaKeys);
            potionsButton.onClick.AddListener(() =>
            {
                if (!_isPotionTab)
                {
                    _isPotionTab = true;
                    potionsButton.image.sprite = activeSprite;
                    ingredientsButton.image.sprite = anActiveSprite;
                    HideCells(_ingredientsKeys);
                    foreach (string formulaKey in _formulaKeys)
                    {
                        if (Progress.gameData.lootData.lootPiecesInDataDictionary.Dictionary.ContainsKey(formulaKey))
                        {
                            RefreshItem(
                                Progress.gameData.lootData.lootPiecesInDataDictionary
                                    .Dictionary[formulaKey]);
                        }
                    }
                }
            });
            ingredientsButton.onClick.AddListener(() =>
            {
                if (_isPotionTab)
                {
                    _isPotionTab = false;
                    potionsButton.image.sprite = anActiveSprite;
                    ingredientsButton.image.sprite = activeSprite;
                    HideCells(_formulaKeys);
                    foreach (string ingredientsKey in _ingredientsKeys)
                    {
                        if (Progress.gameData.lootData.lootPiecesInDataDictionary.Dictionary
                            .ContainsKey(ingredientsKey))
                        {
                            RefreshItem(
                                Progress.gameData.lootData.lootPiecesInDataDictionary
                                    .Dictionary[ingredientsKey]);
                        }
                    }
                }
            });
            addAllPotionsButton.onClick.AddListener(AddAllIngredients);
        }

        private void CreatePotions(List<FormulaStaticData> staticData)
        {
            for (int i = 0; i < staticData.Count; i++)
            {
                _formulaKeys.Add(staticData[i].name);
                _items[staticData[i].name] = Instantiate(cellprefab).GetComponent<CellItem>();
                _items[staticData[i].name].transform.SetParent(content.transform, false);
                _items[staticData[i].name].SetItemSprite(staticData[i].sprite);
                if (Progress.gameData.lootData.lootPiecesInDataDictionary.Dictionary.ContainsKey(staticData[i].name))
                {
                    RefreshItem(Progress.gameData.lootData.lootPiecesInDataDictionary.Dictionary[staticData[i].name]);
                }
                else
                {
                    _items[staticData[i].name].Hide();
                }
            }
        }

        private void HideCells(List<string> keys)
        {
            foreach (string key in keys)
            {
                _items[key].Hide();
            }
        }

        private void CreateIngredients(List<IngredientStaticData> ingredientStaticData)
        {
            for (int i = 0; i < ingredientStaticData.Count; i++)
            {
                _ingredientsKeys.Add(ingredientStaticData[i].name);
                _items[ingredientStaticData[i].name] = Instantiate(cellprefab).GetComponent<CellItem>();
                _items[ingredientStaticData[i].name].transform.SetParent(content.transform, false);
                _items[ingredientStaticData[i].name].SetItemSprite(ingredientStaticData[i].lootIcon);
                if (Progress.gameData.lootData.lootPiecesInDataDictionary.Dictionary.ContainsKey(ingredientStaticData[i]
                        .name))
                {
                    RefreshItem(
                        Progress.gameData.lootData.lootPiecesInDataDictionary
                            .Dictionary[ingredientStaticData[i]?.name]);
                }
                else
                {
                    _items[ingredientStaticData[i].name].Hide();
                }
            }
        }

        private void AddAllIngredients()
        {
            foreach (string ingredientStaticData in _isPotionTab ? _formulaKeys:_ingredientsKeys)
            {
                Progress.gameData.lootData.lootPiecesInDataDictionary.Dictionary[ingredientStaticData] =
                    new Loot(ingredientStaticData, 10);
                RefreshItem(Progress.gameData.lootData.lootPiecesInDataDictionary.Dictionary[ingredientStaticData]);
            }
        }
    }
}