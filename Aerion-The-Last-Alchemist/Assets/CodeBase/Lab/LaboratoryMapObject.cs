using System;
using CodeBase.Map;
using UnityEngine;

namespace CodeBase.Lab
{
    public class LaboratoryMapObject:MonoBehaviour,IActionObject
    {
        public event Action OnAction;
    }
}