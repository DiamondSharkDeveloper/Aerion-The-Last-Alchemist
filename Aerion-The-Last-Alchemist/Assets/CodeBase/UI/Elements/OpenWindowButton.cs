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
        
        public void Init(IWindowService windowService,Action<FormulaStaticData> action)
        {
            _windowService = windowService;
            Button.onClick.AddListener(() => Open(action));
        }
        
        private void Open() =>
            _windowService.Open(WindowId);
        public void Open(Action<FormulaStaticData> action) =>
            _windowService.OpenFormulaBook(action);
    }
}
