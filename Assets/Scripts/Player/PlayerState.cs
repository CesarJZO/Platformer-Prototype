using StatePattern;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerState : State
    {
        protected readonly Player player;

        protected PlayerState(Player player) => this.player = player;
        protected PlayerState(Player player, string animationName) : base(animationName)
        {
            this.player = player;
            AnimationDuration = player.animations.GetLength(animationName);
        }

        public override void Start()
        {
            base.Start();
            player.CrossFade(animationHashName);
        }

        public virtual void ReadInput(InputAction.CallbackContext context, InputCommand command) { }
    }
}
