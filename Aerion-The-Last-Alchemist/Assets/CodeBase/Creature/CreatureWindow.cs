using System;
using System.Reflection.Emit;
using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Creature
{
    public class CreatureWindow : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        public event Action OnClose;

        public void Construct(CreatureTypeId creatureTypeId)
        {
            
        }
       
        void Start()
        {
            closeButton.onClick.AddListener(Close);
        }
        private void Close()
        {
            OnClose?.Invoke();
        }
    }
}