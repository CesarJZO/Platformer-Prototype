using UnityEngine;

namespace Player.States
{
    public class Idle : StateMachineBehaviour
    {
        private PlayerController _player;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _rigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _player.previousSpeed = 0;
            _rigidbody.velocity = Vector2.zero;
        }
    }
}
