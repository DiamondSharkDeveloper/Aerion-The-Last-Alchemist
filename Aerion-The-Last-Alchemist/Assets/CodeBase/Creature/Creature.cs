using System;
using CodeBase.Logic;
using CodeBase.Map;
using UnityEngine;

namespace CodeBase.Creature
{
    public class Creature :MonoBehaviour, IActionObject
    {
        private GameObject _heroObject;
        private LookAtTarget _lookAtTarget;
        public event Action OnAction;
        public void Construct(GameObject hero)
        {
            if (hero)
            {
                _heroObject = hero;
            }
        }

        private void Update()
        {
            transform.LookAt(_heroObject != null ? _heroObject.transform : null);
        }


        
    }
}