using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class JumpState : PlayerState
    {
        public JumpState(Player player) : base(player) { }

        public JumpState(Player player, string animationName) : base(player, animationName)
        {
            animationHashName = Animator.StringToHash(animationName);
        }

        public override void Start()
        {
            base.Start();
            player.Rigidbody.AddForce(Vector2.up * player.settings.JumpForce, ForceMode2D.Impulse);
        }

        public override void FixedUpdate()
        {
            var verticalVelocity = player.Rigidbody.velocity.y;
            var newVelocity = new Vector2(
                player.previousSpeed + player.Input.SmoothAxis * player.settings.Speed * player.settings.JumpAirControl,
                verticalVelocity
            );
            newVelocity.x = Mathf.Clamp(newVelocity.x, -player.settings.MaxSpeed, player.settings.MaxSpeed);
            player.Rigidbody.velocity = newVelocity;

            if (verticalVelocity < 0f)
                player.ChangeState(player.FallState);
        }

        public override void ReadInput(InputAction.CallbackContext context, InputCommand command)
        {
            if (command == InputCommand.Attack)
                player.ChangeState(player.AttackState);
        }

        public override string ToString() => nameof(JumpState);
    }
}
