using System;
using CodeBase.StaticData;
using CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private OpenWindowButton inventoryButton;
        [SerializeField] private OpenWindowButton potionsButton;
        [SerializeField] private OpenWindowButton formulaButton;
        [SerializeField] private Image _overImageSprite;
        private bool _isHoldItem;

        public void Construct(IWindowService windowService, Action<FormulaStaticData> openFormulaAction)
        {
            inventoryButton.Init(windowService);
            potionsButton.gameObject.SetActive(false);
            formulaButton.Init(windowService, openFormulaAction, true);
            SetClearOverCursorImage();
        }

        public void Hide()
        {
         inventoryButton.gameObject.SetActive(false);   
         formulaButton.gameObject.SetActive(false);   
        }
        public void Show()
        {
            inventoryButton.gameObject.SetActive(true);   
            formulaButton.gameObject.SetActive(true);   
        }
        private void Update()
        {
            if (_isHoldItem)
            {
                _overImageSprite.transform.position = new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, 0);
            }
        }

        public void SetOverCursorImage(Sprite sprite)
        {
            _overImageSprite.sprite = sprite;
            _overImageSprite.color=Color.white;
            _isHoldItem = true;
        }

        public void SetClearOverCursorImage()
        {
            _overImageSprite.color = Color.clear;
            
            _isHoldItem = false;
        }
    }
}