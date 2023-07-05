using System;
using UnityEngine;

namespace CodeBase.Map
{
    public class WorldObject:MonoBehaviour
    {
        public event EventHandler<EventArgs> Event;
        private EventArgs _eventArgs;
        public void Construct(EventArgs eventArgs)
        {
            _eventArgs = eventArgs;
        }
        
        public void OnTileEvent()
        {
            Event?.Invoke(this, _eventArgs);
        }
    }
}