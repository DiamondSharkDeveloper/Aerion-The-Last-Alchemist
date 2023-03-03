using System;
using CodeBase.Map;

namespace CodeBase.Services.Input
{
    public interface IInputService: IService
    {
        public event Action<WorldTile> OnTileClick;
    }
}