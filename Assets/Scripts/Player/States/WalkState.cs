using UnityEngine;

namespace Player
{
    public class WalkState : PlayerState
    {
        public WalkState(PlayerController player) : base(player) { }

        public override void FixedUpdate()
        {
            player.rigidbody.velocity = new Vector2(
                player.smoothInput.x * player.settings.speed, player.rigidbody.velocity.y
            );
        }

        public override void LateUpdate()
        {
            var absXInput = Mathf.Abs(player.smoothInput.x);
            if (absXInput > 0f)
                player.animator.speed = absXInput;
        }

        public override void Exit()
        {
            player.animator.speed = 1f;
        }
    }
}
