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
            [field:SerializeField] public float Speed { get; private set; }
            [field:SerializeField] public float MaxSpeed { get; private set; }
            [field:SerializeField] public float JumpForce { get; private set; }
            [field:SerializeField, Range(0f, 1f)] public float JumpAirControl { get; private set; }
            [field:SerializeField, Range(0f, 1f)] public float FallAirControl { get; private set; }
            [field:SerializeField] public float AttackDrag { get; private set; }
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

        [field:Header("Dependencies")]
        [field:SerializeField] public Rigidbody2D Rigidbody { get; private set; }
        [field:SerializeField] public Animator Animator { get; private set; }
        [field:SerializeField] public Animations Animations { get; private set; }
        [field:SerializeField] public PlayerInput Input { get; private set; }

        private Quaternion _currentRotation;

        #region State Machine

        private StateMachine _stateMachine;
        public IdleState IdleState { get; private set; }
        public RunState RunState  { get; private set; }
        public JumpState JumpState  { get; private set; }
        public FallState FallState  { get; private set; }
        public AttackState AttackState  { get; private set; }
        public PlayerState CurrentState => _stateMachine.CurrentState as PlayerState;
        public PlayerState LastState => _stateMachine.LastState as PlayerState;
        public void ChangeState(PlayerState state) => _stateMachine.ChangeState(state);

        #endregion

        #region Unity API

        public void CrossFade(int stateHashName) => Animator.CrossFade(stateHashName, 0f, 0);

        private void Awake()
        {
            IdleState = new IdleState(this, "Idle");
            RunState = new RunState(this, "Run");
            JumpState = new JumpState(this, "Jump");
            FallState = new FallState(this, "Fall");
            AttackState = new AttackState(this);

            _stateMachine = new StateMachine(IdleState);
        }

        private void Update()
        {
            CurrentState.Update();
            if (Input.RawAxis > 0 && (int)_currentRotation.y == 180 || Input.RawAxis < 0 && (int)_currentRotation.y != 180)
                _currentRotation.y = Rigidbody.velocity.x > 0f ? 0f : 180f;
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
