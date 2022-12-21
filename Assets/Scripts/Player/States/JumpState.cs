using UnityEngine;

namespace Player
{
    public class JumpState : PlayerState
    {
        private Vector2 _newVelocity;
        
        public JumpState(PlayerController player) : base(player) { }

        public override void Start()
        {
            player.rigidbody.AddForce(Vector2.up * player.settings.jumpForce, ForceMode2D.Impulse);
        }

        public override void Update()
        {
            var velocity = player.rigidbody.velocity;
            _newVelocity = new Vector2(
                player.previousSpeed + player.smoothInput.x * player.settings.speed * player.settings.jumpAirControl,
                velocity.y
            );
            _newVelocity.x = Mathf.Clamp(_newVelocity.x, -player.settings.maxSpeed, player.settings.maxSpeed);
            
        }

        public override void FixedUpdate()
        {
            player.rigidbody.velocity = _newVelocity;
        }
    }
}
