using System;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Infrastructure.Factory;
using CodeBase.Map;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Creature
{
    public class CreatureWindow : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Button closeButton;
        [SerializeField] private CreatureBalanceBar creatureBalanceBar;
        [SerializeField] private GameObject creatureHolder;
        public event Action OnClose;

        public async void Construct(CreatureStats creatureStats, IGameFactory gameFactory,
            IPersistentProgressService progressService,IStaticDataService staticDataService,IInputService inputService)
        {
            inputService.SetCamera(_camera);
            creatureBalanceBar.SetBarImage(
                progressService.Progress.gameData.CreatureDada.ForCreature(creatureStats.CreatureId));
         GameObject creature= await gameFactory.CreateCreature(creatureStats.TypeId, creatureHolder.transform,creatureHolder.transform.position);
         Quaternion transformLocalRotation;
         transformLocalRotation = creature.transform.localRotation;
         transformLocalRotation.eulerAngles=Vector3.zero;
         creature.transform.localRotation = transformLocalRotation;
        WorldObject worldObject= creature.AddComponent<WorldObject>();
        worldObject.Construct(creatureStats);
        worldObject.Event += (sender, args) =>
        {
            CreatureStats stats = args as CreatureStats;
            Loot loot=progressService.Progress.gameData.lootData._onHoldLoot;
            if (loot!=null)
            {
                foreach (FormulaStaticData formulaStaticData in staticDataService.ForFormulas())
                {
                    if (formulaStaticData.name==loot.name)
                    {
                        stats.Hill(formulaStaticData.potionType,formulaStaticData.efect);
                        stats.CalculateBalance();
                        progressService.Progress.gameData.lootData.Use();
                        creatureBalanceBar.SetBarImage(stats);
                        return;
                    }
                }
                progressService.Progress.gameData.lootData.LetGo();
               
            }
        };
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