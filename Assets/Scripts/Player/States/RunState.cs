using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class RunState : PlayerState
    {
        public RunState(Player player) : base(player) { }

        public RunState(Player player, string animationName) : base(player, animationName)
        {
            animationHashName = Animator.StringToHash(animationName);
        }

        public override void Update()
        {
            if (Mathf.Abs(player.Input.SmoothAxis) < player.Input.deadZone)
                player.ChangeState(player.IdleState);
        }

        public override void FixedUpdate()
        {
            player.Rigidbody.velocity = player.Input.SmoothAxis * player.settings.Speed * Vector2.right;
            if (!player.Grounded)
                player.ChangeState(player.FallState);
        }

        public override void LateUpdate()
        {
            var absXInput = Mathf.Abs(player.Input.SmoothAxis);
            if (absXInput > 0f)
                player.Animator.speed = absXInput;
        }

        public override void ReadInput(InputAction.CallbackContext context, InputCommand command)
        {
            player.ChangeState(command switch
            {
                InputCommand.Attack => player.AttackState,
                InputCommand.Jump => player.JumpState,
                _ => null
            });
        }

        public override void Exit()
        {
            player.Animator.speed = 1f;
        }

        public override string ToString() => nameof(RunState);
    }
}
