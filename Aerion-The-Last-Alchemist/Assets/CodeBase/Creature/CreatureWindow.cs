using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Creature
{
    public class CreatureWindow : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        public event Action OnClose;

       
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