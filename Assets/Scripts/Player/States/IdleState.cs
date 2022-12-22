using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class IdleState : PlayerState
    {
        public IdleState(Player player) : base(player) { }

        public IdleState(Player player, string animationName) : base(player, animationName)
        {
            animationHashName = Animator.StringToHash(animationName);
        }

        public override void Start()
        {
            base.Start();
            player.previousSpeed = 0f;
            player.rigidbody.velocity = Vector2.zero;
        }

        public override void Update()
        {
            if (Mathf.Abs(player.input.SmoothAxis) > player.input.deadZone)
                player.ChangeState(player.runState);
        }

        public override void ReadInput(InputAction.CallbackContext context, InputCommand command)
        {
            player.ChangeState(command switch
            {
                InputCommand.Attack => player.attackState,
                InputCommand.Jump => player.jumpState,
                _ => null
            });
        }

        public override string ToString() => nameof(IdleState);
    }
}
