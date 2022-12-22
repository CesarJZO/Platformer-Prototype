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
            player.rigidbody.AddForce(Vector2.up * player.settings.jumpForce, ForceMode2D.Impulse);
        }

        public override void FixedUpdate()
        {
            var verticalVelocity = player.rigidbody.velocity.y;
            var newVelocity = new Vector2(
                player.previousSpeed + player.input.SmoothAxis * player.settings.speed * player.settings.jumpAirControl,
                verticalVelocity
            );
            newVelocity.x = Mathf.Clamp(newVelocity.x, -player.settings.maxSpeed, player.settings.maxSpeed);
            player.rigidbody.velocity = newVelocity;

            if (verticalVelocity < 0f)
                player.ChangeState(player.fallState);
        }

        public override void ReadInput(InputAction.CallbackContext context, InputCommand command)
        {
            if (command == InputCommand.Attack)
                player.ChangeState(player.attackState);
        }

        public override string ToString() => nameof(JumpState);
    }
}
