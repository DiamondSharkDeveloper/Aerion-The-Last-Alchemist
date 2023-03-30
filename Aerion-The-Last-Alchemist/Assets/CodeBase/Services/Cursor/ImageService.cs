using System;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CodeBase.Services.Cursor
{
    public class ImageService : MonoBehaviour, IImageService
    {
        private Texture2D _defCursor;
        private HUD _hud;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Init(HUD hud)
        {
            _hud = hud;
        }

        public void SetCursorImage(Texture2D sprite)
        {
            UnityEngine.Cursor.SetCursor(sprite, new Vector2(sprite.width / 2, sprite.height / 2), CursorMode.Auto);
        }

        public void SetDefaultCursor(Texture2D sprite)
        {
            _defCursor = sprite;
            SetToDefault();
        }

        public void SetToDefault()
        {
            SetCursorImage(_defCursor);
        }

        public void SetOverCursorImage(Sprite sprite)
        {
            _hud.SetOverCursorImage(sprite);
        }

        public void SetClearOverCursorImage()
        {
          _hud.SetClearOverCursorImage();
        }
    }
}