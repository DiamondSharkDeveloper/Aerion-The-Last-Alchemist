using System;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.UI.Windows;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace CodeBase.Lab
{
    public class LaboratoryWindow : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private CraftZone craftZone;
        public event Action OnClose;

        public void Init([CanBeNull] FormulaStaticData data,IPersistentProgressService progressService,IWindowService windowService)
        {
            if (data)
            {
             craftZone.Init(windowService,progressService,data);   
            }
            else
            {
                craftZone.Init(windowService,progressService);   
            }
            
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