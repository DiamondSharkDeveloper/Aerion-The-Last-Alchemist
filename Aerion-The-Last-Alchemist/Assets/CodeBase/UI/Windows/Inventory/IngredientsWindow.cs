using System;
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
            _onClose += () => {  Progress.gameData.lootData.Changed -= RefreshItem;};
        }

        private void OnDisable()
        {
            Progress.gameData.lootData.Changed -= RefreshItem;
        }
        

        public void Initialize(Dictionary<string,IngredientStaticData> ingredientStaticData,
            List<FormulaStaticData> formulaStaticData,Action<Sprite,string> onHold)
        {
            _isPotionTab = false;
            CreateIngredients(ingredientStaticData,onHold);
            CreatePotions(formulaStaticData,onHold);
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

        private void CreatePotions(List<FormulaStaticData> staticData, Action<Sprite, string> onHold)
        {
            for (int i = 0; i < staticData.Count; i++)
            {
                _formulaKeys.Add(staticData[i].name);
                _items[staticData[i].name] = Instantiate(cellprefab).GetComponent<CellItem>();
                _items[staticData[i].name].transform.SetParent(content.transform, false);
                _items[staticData[i].name].SetItem(staticData[i].sprite,staticData[i].name);
                _items[staticData[i].name].OnClick += onHold;
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

        private void CreateIngredients(Dictionary<string,IngredientStaticData> ingredientStaticData, Action<Sprite, string> onHold)
        {
            foreach (string key in ingredientStaticData.Keys)
            {
                _ingredientsKeys.Add(key);
                _items[key] = Instantiate(cellprefab).GetComponent<CellItem>();
                _items[key].transform.SetParent(content.transform, false);
                _items[key].SetItem(ingredientStaticData[key].lootIcon,key);
                _items[key].OnClick += onHold;

                if (Progress.gameData.lootData.lootPiecesInDataDictionary.Dictionary.ContainsKey(key))
                {
                    RefreshItem(
                        Progress.gameData.lootData.lootPiecesInDataDictionary
                            .Dictionary[ingredientStaticData[key]?.name]);
                }
                else
                {
                    _items[key].Hide();
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