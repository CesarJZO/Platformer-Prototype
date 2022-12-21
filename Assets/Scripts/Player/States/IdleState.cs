using UnityEngine;

namespace Player
{
    public class IdleState : PlayerState
    {
        public IdleState(PlayerController player) : base(player) { }

        public override void Start()
        {
            player.previousSpeed = 0f;
            player.rigidbody.velocity = Vector2.zero;
        }
    }
}
