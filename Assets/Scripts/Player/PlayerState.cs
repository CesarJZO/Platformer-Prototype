using StatePattern;

namespace Player
{
    public class PlayerState : State
    {
        protected PlayerController player;

        public PlayerState(PlayerController player) => this.player = player;
    }
}
