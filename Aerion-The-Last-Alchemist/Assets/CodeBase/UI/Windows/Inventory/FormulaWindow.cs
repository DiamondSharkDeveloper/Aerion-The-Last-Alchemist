using System;
using System.Collections.Generic;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.UI.Windows.Inventory.Formula;
using UnityEngine;

namespace CodeBase.UI.Windows.Inventory
{
    public class FormulaWindow : WindowBase
    {
        [SerializeField] private FormulaBook formulaBook;

        public void Initialize(List<FormulaStaticData> staticData,IPersistentProgressService persistentProgressService,Action<FormulaStaticData> action)
        {
            formulaBook.Initialize(staticData,persistentProgressService,action);
        }
    }
}