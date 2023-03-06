using CodeBase.Enums;
using CodeBase.Services;

namespace CodeBase.UI.Windows
{
    public interface IWindowService : IService
    {
        void Open(WindowId windowId);
    }
}