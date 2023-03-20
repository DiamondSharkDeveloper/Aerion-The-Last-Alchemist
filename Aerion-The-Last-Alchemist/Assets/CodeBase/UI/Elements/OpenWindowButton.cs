using System;
using CodeBase.Enums;
using CodeBase.StaticData;
using CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        public Button Button;
        public WindowId WindowId;
        private IWindowService _windowService;

        public void Init(IWindowService windowService)
        {
            _windowService = windowService;
            Button.onClick.AddListener(Open);
        }
        
        public void Init(IWindowService windowService,Action<FormulaStaticData> action,bool isOnMap)
        {
            _windowService = windowService;
            Button.onClick.AddListener(() => OpenFormulaBook(action,isOnMap));
        }
        
        private void Open() =>
            _windowService.Open(WindowId);
        public void OpenFormulaBook(Action<FormulaStaticData> action,bool isOnMap)=>
            _windowService.OpenFormulaBook(action,isOnMap);
    }
}
