using SurvivalShooter.Core;

namespace SurvivalShooter.States
{
    public class GameStateMachine
    {
        public GameState Current { get; private set; }

        public void ChangeState(GameState next)
        {
            Current?.Exit();
            Current = next;
            Current.Enter();
            GameEvents.RaiseStateChanged(Current.Id);
        }

        public void Tick(float deltaTime) => Current?.Tick(deltaTime);
    }
}
