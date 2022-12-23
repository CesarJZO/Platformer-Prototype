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
            player.Rigidbody.drag = _initialDrag;
            _animationDuration1 = player.Animations.GetLength("Attack1");
            _animationDuration2 = player.Animations.GetLength("Attack2");
        }

        public override void Start()
        {
            player.Rigidbody.drag = player.settings.AttackDrag;

            player.Input.CleanLastInputCommand();

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
            if (player.Input.LastInputCommand == InputCommand.Attack)
                nextState = player.AttackState;
            else if (player.Grounded)
                nextState = player.Input.RawAxis != 0f ? player.RunState : player.IdleState;
            else
                nextState = player.FallState;

            player.ChangeState(nextState);
        }

        public override void Exit()
        {
            player.Rigidbody.drag = _initialDrag;
        }

        public override string ToString() => nameof(AttackState);
    }
}
