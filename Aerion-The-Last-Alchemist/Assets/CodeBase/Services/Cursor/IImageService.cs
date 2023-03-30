using CodeBase.UI.Elements;
using UnityEngine;

namespace CodeBase.Services.Cursor
{
    public interface IImageService : IService
    {
        void SetCursorImage(Texture2D sprite);
        void SetDefaultCursor(Texture2D sprite);
        void SetToDefault();
        void Init(HUD hud);
        void SetOverCursorImage(Sprite sprite);
        void SetClearOverCursorImage();
    }
}