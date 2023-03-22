﻿using System;
using System.Threading.Tasks;
using CodeBase.Enums;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Windows.Inventory;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UIRoot";
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private IGameStateMachine _stateMachine;
        private Transform _uiRoot;
        private readonly IPersistentProgressService _progressService;

        public UIFactory(IAssetProvider assets, IStaticDataService staticData,
            IPersistentProgressService progressService,IGameStateMachine stateMachine)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _stateMachine = stateMachine;
        }

        public void CreateInventory()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Inventory);
            IngredientsWindow window = Object.Instantiate(config.Template, _uiRoot) as IngredientsWindow;
            if (window != null)
            {
              //  _stateMachine.Enter<MenuState>();
                window.Construct(_progressService, () =>
                {
                    _stateMachine.Enter<GameLoopState>();
                });
                window.Initialize(_staticData.ForIngredients(),_staticData.ForFormulas());
            }
        }

        public async Task CreateUIRoot()
        {
            GameObject root = await _assets.Instantiate(UIRootPath);
            _uiRoot = root.transform;
        }

        public void CreatePotions()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Potions);
           PotionsWindow window = Object.Instantiate(config.Template, _uiRoot) as PotionsWindow;
            if (window != null)
            {
              //  _stateMachine.Enter<MenuState>();
                window.Construct(_progressService, () =>
                {
                    _stateMachine.Enter<GameLoopState>();
                });
                window.Initialize(_staticData.ForFormulas());
            }
        }

        public void CreateFormula(Action<FormulaStaticData>action,bool isOnMap)
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Formula);
            FormulaWindow window = Object.Instantiate(config.Template, _uiRoot) as FormulaWindow;
            if (window != null)
            {
                // if (isOnMap)
                // {
                //     _stateMachine.Enter<MenuState>();
                // }
               
                window.Construct(_progressService, () =>
                {
                    if (isOnMap)
                    {
                        _stateMachine.Enter<GameLoopState>();
                    }
                });
                window.Initialize(_staticData.ForFormulas(),_progressService,action);
            }
        }
    }
}