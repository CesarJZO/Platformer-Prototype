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
                if (Mathf.Abs(player.Rigidbody.velocity.x) > player.Input.deadZone)
                    player.ChangeState(player.RunState);
                else
                    player.ChangeState(player.IdleState);
                return;
            }

            var velocity = player.Rigidbody.velocity;
            var newVelocity = new Vector2(
                player.previousSpeed + player.Input.SmoothAxis * player.settings.Speed * player.settings.FallAirControl,
                velocity.y
            );
            newVelocity.x = Mathf.Clamp(newVelocity.x, -player.settings.MaxSpeed, player.settings.MaxSpeed);
        }

        public override void ReadInput(InputAction.CallbackContext context, InputCommand command)
        {
            if (command == InputCommand.Attack)
                player.ChangeState(player.AttackState);
        }

        public override void Exit()
        {
            player.Rigidbody.velocity = player.Input.RawAxis * Vector2.right;
        }

        public override string ToString() => nameof(FallState);
    }
}
