using UnityEngine;

namespace Player
{
    public class AttackState : PlayerState
    {
        private float _initialDrag;

        private readonly int _animationHashName1 = Animator.StringToHash("Attack1");
        private readonly float _animationDuration1;
        private readonly int _animationHashName2 = Animator.StringToHash("Attack2");
        private readonly float _animationDuration2;

        public AttackState(Player player) : base(player)
        {
            player.rigidbody.drag = _initialDrag;
            _animationDuration1 = player.animations.GetLength("Attack1");
            _animationDuration2 = player.animations.GetLength("Attack2");
        }

        public override void Start()
        {
            player.rigidbody.drag = player.settings.attackDrag;

            player.input.CleanLastInputCommand();

            if (player.LastState == this)
            {
                player.CrossFade(_animationHashName2);
                AnimationDuration = _animationDuration1;
            }
            else
            {
                player.CrossFade(_animationHashName1);
                AnimationDuration = _animationDuration2;
            }

            stateTime = AnimationDuration + Time.time;
        }

        public override void Update()
        {
            if (Time.time < stateTime) return;

            PlayerState nextState;
            if (player.input.LastInputCommand == InputCommand.Attack)
                nextState = player.attackState;
            else if (player.Grounded)
                nextState = player.input.RawAxis != 0f ? player.runState : player.idleState;
            else
                nextState = player.fallState;

            player.ChangeState(nextState);
        }

        public override void Exit()
        {
            player.rigidbody.drag = _initialDrag;
        }

        public override string ToString() => nameof(AttackState);
    }
}
