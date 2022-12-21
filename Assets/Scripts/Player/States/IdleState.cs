using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class IdleState : PlayerState
    {
        public IdleState(PlayerController player) : base(player)
        {
            animationHashName = Animator.StringToHash("Idle");
        }

        public override void Start()
        {
            player.previousSpeed = 0f;
            player.rigidbody.velocity = Vector2.zero;
            player.CrossFade(animationHashName);
        }

        public override void Update()
        {
            if (Mathf.Abs(player.input.SmoothAxis) > player.input.deadZone)
                player.ChangeState(player.walkState);
        }

        public override void ReadInput(InputAction.CallbackContext context, InputCommand command)
        {
            if (command == InputCommand.Jump)
                player.ChangeState(player.jumpState);
        }

        public override string ToString() => nameof(IdleState);
    }
}
