using CodeBase.Logic;

namespace CodeBase.Hero
{
    public interface IAnimationStateReader
    {
        void EnteredState(int stateHash);
        void ExitedState(int stateHash);
        AnimatorState State { get; }
    }
}