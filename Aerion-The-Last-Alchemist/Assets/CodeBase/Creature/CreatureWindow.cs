using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Creature
{
    public class CreatureWindow : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private CreatureBalanceBar creatureBalanceBar;
        [SerializeField] private GameObject creatureHolder;
        public event Action OnClose;

        public async void Construct(CreatureStats creatureStats, IGameFactory gameFactory,
            IPersistentProgressService progressService)
        {
            creatureBalanceBar.SetBarImage(
                progressService.Progress.gameData.CreatureDada.ForCreature(creatureStats.CreatureId));
         GameObject creature= await gameFactory.CreateCreature(creatureStats.TypeId, creatureHolder.transform,creatureHolder.transform.position);
         Quaternion transformLocalRotation;
         transformLocalRotation = creature.transform.localRotation;
         transformLocalRotation.eulerAngles=Vector3.zero;
         creature.transform.localRotation = transformLocalRotation;
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