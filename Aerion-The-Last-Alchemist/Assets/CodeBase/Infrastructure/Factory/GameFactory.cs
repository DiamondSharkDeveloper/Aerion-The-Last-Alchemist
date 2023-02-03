using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _persistentProgressService;
        private GameObject _heroGameObject;
        private readonly IGameStateMachine _stateMachine;

        public GameFactory(
            IAssetProvider assets, 
            IStaticDataService staticData, 
            IRandomService randomService, 
            IPersistentProgressService persistentProgressService, 
            IGameStateMachine stateMachine)
        {
            _assets = assets;
            _staticData = staticData;
            _randomService = randomService;
            _persistentProgressService = persistentProgressService;
            _stateMachine = stateMachine;
        }
        public Task<GameObject> CreateHero(Vector3 at)
        {
            throw new System.NotImplementedException();
        }

        public Task<GameObject> CreateHud()
        {
            throw new System.NotImplementedException();
        }
        public void Cleanup()
        {
            throw new System.NotImplementedException();
        }
        public async Task WarmUp()
        {
            throw new System.NotImplementedException();
        }


        public Task CreateLevelTransfer(Vector3 at)
        {
            throw new System.NotImplementedException();
        }
    }
}