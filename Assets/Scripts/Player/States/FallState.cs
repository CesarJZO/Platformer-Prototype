using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class FallState : PlayerState
    {
        public FallState(Player player) : base(player) { }

        public FallState(Player player, string animationName) : base(player, animationName)
        {
            animationHashName = Animator.StringToHash(animationName);
        }

        public override void FixedUpdate()
        {
            if (player.Grounded)
            {
                if (Mathf.Abs(player.rigidbody.velocity.x) > player.input.deadZone)
                    player.ChangeState(player.runState);
                else
                    player.ChangeState(player.idleState);
                return;
            }

            var velocity = player.rigidbody.velocity;
            var newVelocity = new Vector2(
                player.previousSpeed + player.input.SmoothAxis * player.settings.speed * player.settings.fallAirControl,
                velocity.y
            );
            newVelocity.x = Mathf.Clamp(newVelocity.x, -player.settings.maxSpeed, player.settings.maxSpeed);
        }

        public override void ReadInput(InputAction.CallbackContext context, InputCommand command)
        {
            if (command == InputCommand.Attack)
                player.ChangeState(player.attackState);
        }

        public override void Exit()
        {
            player.rigidbody.velocity = player.input.RawAxis * Vector2.right;
        }

        public override string ToString() => nameof(FallState);
    }
}
