using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Menu;
using CodeBase.Services;
using CodeBase.StaticData;

namespace CodeBase.Infrastructure.Factory
{
    public interface IUIFactory: IService
    {
        void CreateInventory();
        Task CreateUIRoot();
        void CreatePotions();
        void CreateFormula(Action<FormulaStaticData> action, bool isOnMap);
        MainMenuWindow CreateMainMenu(List<MenuButtons> menuButtonsList,bool isGameRun);
    }
}