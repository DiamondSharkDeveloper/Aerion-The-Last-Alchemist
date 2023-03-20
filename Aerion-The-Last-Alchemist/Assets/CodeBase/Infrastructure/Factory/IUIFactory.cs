using System;
using System.Threading.Tasks;
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
    }
}