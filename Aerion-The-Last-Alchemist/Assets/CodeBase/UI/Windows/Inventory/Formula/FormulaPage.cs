using System;
using CodeBase.Data;
using CodeBase.StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Inventory.Formula
{
    public class FormulaPage : MyPage
    {
        [SerializeField] private CellItem ingredientPrefab;
        [SerializeField] private TextMeshProUGUI potionNameText;
        [SerializeField] private Button _brewButton;
        [SerializeField] private Sprite _brewButtonActiveSprite;
        [SerializeField] private Image _brewButtonImage;
        [SerializeField] private GameObject ingredientsHolder;
        [SerializeField] private Sprite backItemSprite;

        public void SetPage(FormulaStaticData staticData, LootData lootData, Action<FormulaStaticData> brewAction)
        {
            potionNameText.text = staticData.potionType + " Potion" + " Level " + staticData.potionLevel;
            potion.SetItem(staticData.sprite,staticData.potionType.ToString());
            int avaliableIngedientsCount = 0;
            for (int i = 0; i < staticData.ingredients.Count; i++)
            {
                CellItem item = Instantiate(ingredientPrefab, ingredientsHolder.transform, false);
                item.SetItem(staticData.ingredients[i].lootIcon,staticData.ingredients[i].name);
                item.SetItemBackSprite(backItemSprite);
                if (lootData.lootPiecesInDataDictionary.Dictionary.ContainsKey(staticData.ingredients[i].name)&&lootData.lootPiecesInDataDictionary.Dictionary[staticData.ingredients[i].name].value > 0)
                {
                    avaliableIngedientsCount++;
                }
            }

            if (avaliableIngedientsCount == staticData.ingredients.Count)
            {
                _brewButtonImage.sprite = _brewButtonActiveSprite;
                _brewButton.onClick.AddListener(() => { brewAction?.Invoke(staticData); });
            }
        }
    }
}