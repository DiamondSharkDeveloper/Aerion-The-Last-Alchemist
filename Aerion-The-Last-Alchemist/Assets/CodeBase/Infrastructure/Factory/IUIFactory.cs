using System.Threading.Tasks;
using CodeBase.Services;

namespace CodeBase.Infrastructure.Factory
{
    public interface IUIFactory: IService
    {
        void CreateInventory();
        Task CreateUIRoot();
        void CreatePotions();
        void CreateFormula();
    }
}