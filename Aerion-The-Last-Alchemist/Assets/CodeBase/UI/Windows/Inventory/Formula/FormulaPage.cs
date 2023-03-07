using CodeBase.StaticData;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows.Inventory.Formula
{
    public  class FormulaPage:MyPage
    {
        [SerializeField] private TextMeshProUGUI potionNameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private GameObject ingredientsHolder;

        public void SetPage(FormulaStaticData staticData)
        {
            potionNameText.text = staticData.potionType + " Potion" + " Level" + staticData.potionLevel;
            descriptionText.text = staticData.description;
            potion.SetItemSprite(staticData.sprite);
            for (var i = 0; i < staticData.ingredients.Count; i++)
            {
                CellItem item = Instantiate(potion, ingredientsHolder.transform, false);
                item.SetItemSprite(staticData.ingredients[i].lootIcon);
            }
        }
      
    }
}