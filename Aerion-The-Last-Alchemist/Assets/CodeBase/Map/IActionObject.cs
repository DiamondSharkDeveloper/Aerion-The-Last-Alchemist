using System;
using UnityEngine;

namespace CodeBase.Map
{
    public interface IActionObject
    {
        public event Action OnAction;
    }
}