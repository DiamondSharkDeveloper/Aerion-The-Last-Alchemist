using System;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.UI.Windows;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Lab
{
    public class LaboratoryWindow : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private CraftZone craftZone;
        [SerializeField] private Camera _camera;
        public event Action OnClose;

        [Obsolete("Obsolete")]
        public void Init([CanBeNull] FormulaStaticData data, IPersistentProgressService progressService,
            IWindowService windowService, IInputService inputService)
        {
            inputService.SetCamera(_camera);
            if (data!=null)
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