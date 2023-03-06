using CodeBase.Enums;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;

namespace CodeBase.UI.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;

        public WindowService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Open(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.None:
                    break;
                case WindowId.Inventory:
                    _uiFactory.CreateInventory();
                    break;
                case WindowId.Potions:
                    _uiFactory.CreatePotions();
                    break;
                case WindowId.Formula:
                    _uiFactory.CreateFormula();
                    break;
            }
        }
    }
}