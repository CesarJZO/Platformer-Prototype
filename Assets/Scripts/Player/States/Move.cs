using Unity.VisualScripting;
using UnityEngine;

namespace Player.States
{
    public class Move : StateMachineBehaviour
    {
        private static readonly int XInputID = Animator.StringToHash("XInput");
        private PlayerController _player;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _rigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.speed = 1;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _rigidbody.velocity = new Vector2(_player.smoothInput.x * _player.speed, _rigidbody.velocity.y);
            var absXInput = animator.GetFloat(XInputID);
            if (absXInput > 0)
                animator.speed = absXInput;
        }
    }
}
