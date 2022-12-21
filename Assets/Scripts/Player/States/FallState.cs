using UnityEngine;

namespace Player
{
    public class FallState : PlayerState
    {
        public FallState(PlayerController player) : base(player) { }

        public override void FixedUpdate()
        {
            var velocity = player.rigidbody.velocity;
            var newVelocity = new Vector2(
                player.previousSpeed + player.smoothInput.x * player.settings.speed * player.settings.fallAirControl,
                velocity.y
            );
            newVelocity.x = Mathf.Clamp(newVelocity.x, -player.settings.maxSpeed, player.settings.maxSpeed);
        }

        public override void Exit()
        {
            // _rigidbody.velocity = new Vector2(_player.rawInput.x, _rigidbody.velocity.y);
            player.rigidbody.velocity = player.rawInput * Vector2.right;
        }
    }
}
