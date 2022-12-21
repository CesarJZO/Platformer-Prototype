using StatePattern;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerState : State
    {
        protected readonly PlayerController player;

        protected PlayerState(PlayerController player) => this.player = player;

        public virtual void ReadInput(InputAction.CallbackContext context, InputCommand command) { }
    }
}
