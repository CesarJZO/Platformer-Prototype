using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class WalkState : PlayerState
    {
        public WalkState(PlayerController player) : base(player)
        {
            animationHashName = Animator.StringToHash("Move");
        }

        public override void Start()
        {
            player.CrossFade(animationHashName);
        }

        public override void Update()
        {
            if (Mathf.Abs(player.input.SmoothAxis) < player.input.deadZone)
                player.ChangeState(player.idleState);
        }

        public override void FixedUpdate()
        {
            player.rigidbody.velocity = player.input.SmoothAxis * player.settings.speed * Vector2.right;
            if (!player.Grounded)
                player.ChangeState(player.fallState);
        }

        public override void LateUpdate()
        {
            var absXInput = Mathf.Abs(player.input.SmoothAxis);
            if (absXInput > 0f)
                player.animator.speed = absXInput;
        }

        public override void ReadInput(InputAction.CallbackContext context, InputCommand command)
        {
            if (command == InputCommand.Jump)
                player.ChangeState(player.jumpState);
        }
        
        public override void Exit()
        {
            player.animator.speed = 1f;
        }
        
        public override string ToString() => nameof(WalkState);
    }
}
