using System;
using System.Collections.Generic;
using CodeBase.StaticData;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows.Inventory.Formula
{
    internal class FormulasPage:MyPage
    {
        [SerializeField] private GameObject potionsHolder;
        public void SetPage(List<FormulaStaticData> formulaStaticDats)
        {
            for (var i = 0; i < formulaStaticDats.Count; i++)
            {
                CellItem item = Instantiate(potion, potionsHolder.transform, false);
                item.SetItemSprite(formulaStaticDats[i].sprite);
            }
        }
    }
}