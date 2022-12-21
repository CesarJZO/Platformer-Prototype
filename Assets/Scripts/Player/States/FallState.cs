using UnityEngine;

namespace Player
{
    public class FallState : PlayerState
    {
        public FallState(PlayerController player) : base(player)
        {
            animationHashName = Animator.StringToHash("Fall");
        }

        public override void Start()
        {
            player.CrossFade(animationHashName);
        }

        public override void FixedUpdate()
        {
            if (player.Grounded)
            {
                if (Mathf.Abs(player.rigidbody.velocity.x) > player.input.deadZone)
                    player.ChangeState(player.walkState);
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

        public override void Exit()
        {
            player.rigidbody.velocity = player.input.RawAxis * Vector2.right;
        }
        
        public override string ToString() => nameof(FallState);
    }
}
