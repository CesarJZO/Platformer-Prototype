using UnityEngine;

namespace Player.States
{
    public class Fall : StateMachineBehaviour
    {
        [Range(0f, 1f)] public float airControl;
        private PlayerController _player;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _rigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        }
        
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var velocity = _rigidbody.velocity;
            var newVelocity = new Vector2(_player.previousSpeed + _player.smoothInput.x * _player.speed * airControl, velocity.y);
            _rigidbody.velocity = newVelocity;
        }
    }
}
