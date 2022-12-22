using System;
using Settings;
using StatePattern;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [Serializable]
        public class Settings
        {
            public float speed;
            public float maxSpeed;
            public float jumpForce;
            [Range(0f, 1f)] public float jumpAirControl;
            [Range(0f, 1f)] public float fallAirControl;
            public float attackDrag;
        }

        public Settings settings;

        [Header("Debug")]
        [SerializeField] private bool showStats;
        [SerializeField] private int fontSize;
        [SerializeField] private Vector2 textPosition;

        [Header("Physics")]
        [HideInInspector] public float previousSpeed;
        [SerializeField] private float groundDistance;
        [SerializeField] private LayerMask groundMask;
        public RaycastHit2D Grounded => Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);

        [Header("Dependencies")]
        public new Rigidbody2D rigidbody;
        public Animator animator;
        public Animations animations;
        public PlayerInput input;

        private Quaternion _currentRotation;


        #region State Machine

        private StateMachine _stateMachine;
        public IdleState idleState;
        public RunState runState;
        public JumpState jumpState;
        public FallState fallState;
        public AttackState attackState;
        public PlayerState CurrentState => _stateMachine.CurrentState as PlayerState;
        public PlayerState LastState => _stateMachine.LastState as PlayerState;
        public void ChangeState(PlayerState state) => _stateMachine.ChangeState(state);

        #endregion

        #region Unity API

        public void CrossFade(int stateHashName) => animator.CrossFade(stateHashName, 0f, 0);

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();

            idleState = new IdleState(this, "Idle");
            runState = new RunState(this, "Run");
            jumpState = new JumpState(this, "Jump");
            fallState = new FallState(this, "Fall");
            attackState = new AttackState(this);

            _stateMachine = new StateMachine(idleState);
        }

        private void Update()
        {
            CurrentState.Update();
            if (input.RawAxis > 0 && (int)_currentRotation.y == 180 || input.RawAxis < 0 && (int)_currentRotation.y != 180)
                _currentRotation.y = rigidbody.velocity.x > 0f ? 0f : 180f;
        }

        private void FixedUpdate()
        {
            CurrentState.FixedUpdate();
        }

        private void LateUpdate()
        {
            CurrentState.LateUpdate();
            transform.rotation = _currentRotation;
        }

        private void OnGUI()
        {
            if (!showStats) return;
            GUI.Label(
                new Rect(textPosition, Vector2.one),
                @$"Stats
Current: {CurrentState}",
                new GUIStyle
                {
                    fontSize = fontSize,
                    normal = { textColor = Color.white}
                }
            );
        }

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(position, position + Vector3.down * groundDistance);
        }

        #endregion
    }
}
