using CodeBase.Lab;

namespace CodeBase.Infrastructure.States
{
    public class MenuState:IState
    {
        private LaboratoryWindow _laboratoryWindow;
        public void Exit()
        {
        }

        public bool IsOnPause()
        {
            return true;
        }

        public void Enter()
        {
        }
    }
}