using System;
using System.Reflection.Emit;
using CodeBase.Logic;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.Creature
{
    public class Creature:MonoBehaviour
    {
        private GameObject _heroObject;
        private LookAtTarget _lookAtTarget;
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