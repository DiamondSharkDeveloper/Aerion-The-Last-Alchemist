using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private OpenWindowButton inventoryButton;
        [SerializeField] private OpenWindowButton potionsButton;

        public void Construct(IWindowService windowService)
        {
            inventoryButton.Init(windowService);
            potionsButton.Init(windowService);
        }
    }
}
