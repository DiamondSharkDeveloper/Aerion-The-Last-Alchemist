using System;
using UnityEngine;

namespace CodeBase.Map
{
    public class WorldObject:MonoBehaviour
    {
        public event EventHandler<EventArgs> TileEvent;
        private EventArgs _eventArgs;
        public void Construct(EventArgs eventArgs)
        {
            _eventArgs = eventArgs;
        }

        public void OnTileEvent()
        {
            TileEvent?.Invoke(this, _eventArgs);
        }
    }
}