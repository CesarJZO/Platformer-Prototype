using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        [Header("Settings")]
        
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;

        [Header("Input")]
        
        [SerializeField] private float smoothTime;

        private Vector2 _rawInput;
        private Vector2 _smoothInput;
        [SerializeField] private float deadZone;
        public float XInput { get; private set; }
        public float YInput { get; private set; }

        #region Animator

        private Animator _animator;
        private readonly int _moveID = Animator.StringToHash("XInput");
        private readonly int _jumpID = Animator.StringToHash("Jump");

        #endregion

        #region Physics

        [Header("Physics")] [SerializeField] private float groundDistance;
        [SerializeField] private LayerMask groundMask;
        public RaycastHit2D Grounded => Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);
        #endregion

        #region Components

        private Rigidbody2D _rigidbody;
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private InputAction _jumpAction;

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            Instance = this;
            _animator = GetComponentInChildren<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
            _moveAction = _playerInput.actions["Move"];
            _jumpAction = _playerInput.actions["Jump"];
        }

        private void Update()
        {
            var currentVelocity = Vector2.zero;


            _rawInput = _moveAction.ReadValue<Vector2>();
            _smoothInput = Vector2.SmoothDamp(_smoothInput, _rawInput, ref currentVelocity, smoothTime);
            
            if (_smoothInput.magnitude <= deadZone)
                _smoothInput = Vector2.zero;

            XInput = _smoothInput.x;
            YInput = _smoothInput.y;
            if (_jumpAction.WasPressedThisFrame() && Grounded)
            {
                _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                _animator.SetTrigger(_jumpID);
            }
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector2(XInput * speed, _rigidbody.velocity.y);
        }

        private void LateUpdate()
        {
            _animator.SetFloat(_moveID, XInput);
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
