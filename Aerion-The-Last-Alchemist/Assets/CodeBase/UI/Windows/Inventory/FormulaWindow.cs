using System.Collections.Generic;
using CodeBase.StaticData;
using CodeBase.UI.Windows.Inventory.Formula;
using UnityEngine;

namespace CodeBase.UI.Windows.Inventory
{
    public class FormulaWindow : WindowBase
    {
        [SerializeField] private FormulaBook formulaBook;

        public void Initialize(List<FormulaStaticData> staticDatas)
        {
            formulaBook.Initialize(staticDatas);
        }
    }
}