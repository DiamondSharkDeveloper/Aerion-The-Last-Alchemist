using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroAnimator: MonoBehaviour,IAnimationStateReader
    {
        [SerializeField] public Animator _animator;
        private static readonly int MoveHash = Animator.StringToHash("Walk");
        private static readonly int GrabHash = Animator.StringToHash("Grab");
        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _run = Animator.StringToHash("Run");
        public AnimatorState State { get; private set; }
        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;
        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void PlayMoveAnimation(bool isMove)
        {
            _animator.SetBool(MoveHash,isMove);
        }
        public void PlayGrab()
        {
            _animator.SetTrigger(GrabHash);
        }
        public void ExitedState(int stateHash)
        {
            StateExited?.Invoke(StateFor(stateHash));
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _idleStateHash)
            {
                state = AnimatorState.Idle;
            }
            else if (stateHash == _run)
            {
                state = AnimatorState.Run;
            }
            else
            {
                state = AnimatorState.Unknown;
            }
            return state;
        }
        public void ResetToIdle()
        {
            _animator.Play(_idleStateHash, -1);
        }
    }
}