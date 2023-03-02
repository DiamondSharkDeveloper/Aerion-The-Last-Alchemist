using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
    public class Hero : MonoBehaviour
    {
        private HeroMove _heroMove;
        [SerializeField] private HeroAnimator heroAnimator;

        public void Construct(IInputService inputService)
        {
            _heroMove = gameObject.AddComponent<HeroMove>();
            _heroMove.Construct(inputService);
            _heroMove._isMove += heroAnimator.PlayMoveAnimation;
            _heroMove.OnInteractiveObject += () => { heroAnimator.PlayGrab();};
        }
    }
}