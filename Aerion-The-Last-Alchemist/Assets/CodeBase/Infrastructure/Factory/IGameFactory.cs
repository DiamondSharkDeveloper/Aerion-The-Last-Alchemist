using System.Threading.Tasks;
using CodeBase.Services;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        Task<GameObject> CreateHero(Vector3 at);
        Task<GameObject> CreateHud();
        void Cleanup();
        Task WarmUp();
        Task CreateLevelTransfer(Vector3 at);
    }
}