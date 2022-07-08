using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings")]
        public float speed;
        public float maxSpeed;
        public float jumpForce;

        [Header("Input")]
        [SerializeField] private float smoothTime;
        [HideInInspector] public Vector2 rawInput;
        [HideInInspector] public Vector2 smoothInput;
        [SerializeField] private float deadZone;
        
        #region Physics

        [Header("Physics")]
        [HideInInspector] public float previousSpeed;
        [SerializeField] private float groundDistance;
        [SerializeField] private LayerMask groundMask;
        private RaycastHit2D Grounded => Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);
        
        #endregion
        
        #region Sprites

        private Quaternion _currentRotation;
        private Animator _animator;
        private static readonly int MoveID = Animator.StringToHash("Abs XInput");
        private static readonly int JumpID = Animator.StringToHash("Jump");
        private static readonly int GroundedID = Animator.StringToHash("Grounded");
        private static readonly int YVelocityID = Animator.StringToHash("Vertical Velocity");
        private static readonly int XVelocityID = Animator.StringToHash("Abs Horizontal Velocity");

        #endregion
        
        #region Components

        public new Rigidbody2D rigidbody;
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private InputAction _jumpAction;

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
            _moveAction = _playerInput.actions["Move"];
            _jumpAction = _playerInput.actions["Jump"];
        }

        private void Update()
        {
            var currentVelocity = Vector2.zero;
            
            rawInput = _moveAction.ReadValue<Vector2>();
            smoothInput = Vector2.SmoothDamp(smoothInput, rawInput, ref currentVelocity, smoothTime);
            
            if (smoothInput.magnitude <= deadZone)
                smoothInput = Vector2.zero;
            
            if (Mathf.Abs(rigidbody.velocity.x) > deadZone)
                _currentRotation.y = rigidbody.velocity.x > 0 ? 0 : 180;
            
            // Jump pressed and grounded?
            if (!_jumpAction.WasPressedThisFrame() || !Grounded) return;
            
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Jump") || stateInfo.IsName("Fall")) return;
            
            previousSpeed = rigidbody.velocity.x;
            _animator.SetTrigger(JumpID);
        }

        private void LateUpdate()
        {
            _animator.SetFloat(MoveID, Mathf.Abs(smoothInput.x));
            _animator.SetFloat(XVelocityID, Mathf.Abs(rigidbody.velocity.x));
            _animator.SetFloat(YVelocityID, rigidbody.velocity.y);
            _animator.SetBool(GroundedID, Grounded);
            transform.rotation = _currentRotation;
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
