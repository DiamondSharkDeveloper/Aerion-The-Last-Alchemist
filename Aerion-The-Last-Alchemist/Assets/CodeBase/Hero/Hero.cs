using System;
using CodeBase.Map;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
    public class Hero : MonoBehaviour
    {
        private HeroMove _heroMove;
        [SerializeField] private HeroAnimator heroAnimator;
        public void Construct()
        {
            _heroMove = gameObject.AddComponent<HeroMove>();
            _heroMove.Construct();
            _heroMove._isMove += heroAnimator.PlayMoveAnimation;
            _heroMove.OnInteractiveObject +=() => { heroAnimator.PlayGrab(() =>
            {
                _heroMove.canMove=true;
            });};
        }
        public void Move(EventArgs  eventArgs)
        {
            _heroMove.Move(eventArgs as MyTile);
        }
    }
}