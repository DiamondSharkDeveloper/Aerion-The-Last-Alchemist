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
        private WorldData _worldData;
        private Loot _loot;

        private const float DelayBeforeDestroying = 1f;

        private string _id;

        private bool _pickedUp = false;

        public void Construct(WorldData worldData) =>
            _worldData = worldData;

        public void Initialize(Loot loot) =>
            _loot = loot;

        private void Start()
        {
            OnAction = () => { Pickup(); };
        }


        public void UpdateProgress(PlayerProgress progress)
        {
            if (_pickedUp)
                return;

            LootPieceDataDictionary lootPiecesOnScene = progress.WorldData.LootData.LootPiecesOnScene;

            if (!lootPiecesOnScene.Dictionary.ContainsKey(_id))
                lootPiecesOnScene.Dictionary
                    .Add(_id, new LootPieceData(transform.position.AsVectorData(), _loot));
        }

        public void LoadProgress(PlayerProgress progress)
        {
        }

        public void Pickup()
        {
           // UpdateWorldData();
            
           // PlayPickupFx();
            //HideLootObject();
            Destroy(gameObject, DelayBeforeDestroying);
        }

        private void UpdateWorldData()
        {
            // UpdateCollectedLootAmount();
            // RemoveLootPieceFromSavedPieces();
        }

        // private void UpdateCollectedLootAmount() =>
        //   _worldData.LootData.Collect(_loot);

        private void RemoveLootPieceFromSavedPieces()
        {
            LootPieceDataDictionary savedLootPieces = _worldData.LootData.LootPiecesOnScene;

            if (savedLootPieces.Dictionary.ContainsKey(_id))
                savedLootPieces.Dictionary.Remove(_id);
        }

        private void HideLootObject() =>
            lootObjectRenderer.enabled=false;

        private void PlayPickupFx() =>
            pickupFxPrefab.SetActive(true);
    }
}