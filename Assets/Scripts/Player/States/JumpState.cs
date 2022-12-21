using UnityEngine;

namespace Player
{
    public class JumpState : PlayerState
    {
        public JumpState(PlayerController player) : base(player)
        {
            animationHashName = Animator.StringToHash("Jump");
        }

        public override void Start()
        {
            player.rigidbody.AddForce(Vector2.up * player.settings.jumpForce, ForceMode2D.Impulse);
            player.CrossFade(animationHashName);
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
        
        public override string ToString() => nameof(JumpState);
    }
}
