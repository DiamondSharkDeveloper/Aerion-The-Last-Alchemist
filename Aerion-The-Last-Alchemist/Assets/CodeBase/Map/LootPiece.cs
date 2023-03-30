using System;
using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Map;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootPiece : MonoBehaviour
    {
        public MeshRenderer lootObjectRenderer;
        public GameObject pickupFxPrefab;
        public Action OnAction;
        private GameData _gameData;
        private Loot _loot;

        private const float DelayBeforeDestroying = 1f;

        private string _id;

        private bool _pickedUp = false;

        public void Construct(GameData gameData) =>
            _gameData = gameData;

        public void Initialize(Loot loot) =>
            _loot = loot;

        private void Start()
        {
            OnAction = () => { Pickup(); };
        }


        public void LoadProgress(PlayerProgress progress)
        {
        }

        public void Pickup()
        {
            Destroy(gameObject, DelayBeforeDestroying);
            UpdateWorldData();
            OnAction = null;
        }

        private void UpdateWorldData()
        {
            UpdateCollectedLootAmount();
        }

        private void UpdateCollectedLootAmount()
        {
            _gameData.lootData?.Collect(_loot);
        }
    }
}