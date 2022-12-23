using UnityEngine;

namespace Player
{
    public class AttackState : PlayerState
    {
        private float _initialDrag;

        public AttackState(Player player) : base(player)
        {
            player.Rigidbody.drag = _initialDrag;
        }

        public AttackState(Player player, string animationName) : base(player, animationName)
        {
            player.Rigidbody.drag = _initialDrag;
        }

        public override void Start()
        {
            base.Start();
            player.Rigidbody.drag = player.settings.AttackDrag;
            stateTime = AnimationDuration + Time.time;
        }

        public override void Update()
        {
            if (Time.time < stateTime) return;

            PlayerState nextState = player.Grounded
                ? player.Input.RawAxis != 0f ? player.runState : player.idleState
                : player.fallState;

            player.ChangeState(nextState);
        }

        public override void Exit()
        {
            player.Rigidbody.drag = _initialDrag;
        }

        public override string ToString() => nameof(AttackState);
    }
}
