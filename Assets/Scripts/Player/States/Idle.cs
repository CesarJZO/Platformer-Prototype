using UnityEngine;

namespace Player.States
{
    public class Idle : StateMachineBehaviour
    {
        public PlayerController player;
        public Rigidbody2D rigidbody;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            rigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            rigidbody.velocity = Vector2.zero;
        }
    }
}
