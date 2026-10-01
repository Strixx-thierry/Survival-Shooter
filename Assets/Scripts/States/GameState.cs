using SurvivalShooter.Core;

namespace SurvivalShooter.States
{
    /// <summary>State pattern base class.</summary>
    public abstract class GameState
    {
        protected readonly GameManager Game;
        public abstract GameStateId Id { get; }

        protected GameState(GameManager game) { Game = game; }

        public virtual void Enter() { }
        public virtual void Tick(float deltaTime) { }
        public virtual void Exit() { }
    }

    public class MenuState : GameState
    {
        public MenuState(GameManager game) : base(game) { }
        public override GameStateId Id => GameStateId.Menu;
    }

    public class PlacementState : GameState
    {
        public PlacementState(GameManager game) : base(game) { }
        public override GameStateId Id => GameStateId.Placement;
    }

    public class PlayState : GameState
    {
        public PlayState(GameManager game) : base(game) { }
        public override GameStateId Id => GameStateId.Play;

        public override void Enter() => Game.BeginSession();

        public override void Tick(float deltaTime) => Game.TickSession(deltaTime);
    }

    public class EndState : GameState
    {
        public EndState(GameManager game) : base(game) { }
        public override GameStateId Id => GameStateId.End;
    }
}
