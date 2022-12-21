﻿using System;
using StatePattern;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [Serializable]
        public class Settings
        {
            public float speed;
            public float maxSpeed;
            public float jumpForce;
            [Range(0f, 1f)] public float jumpAirControl;
            [Range(0f, 1f)] public float fallAirControl;
        }

        public Settings settings;

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
        public Animator animator;
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

        #region StateMachine

        private StateMachine _stateMachine;
        public IdleState idleState;
        public WalkState walkState;
        public JumpState jumpState;
        public FallState fallState;
        public void ChangeState(PlayerState state) => _stateMachine.ChangeState(state);

        #endregion
        
        #region MonoBehaviour Methods

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
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
            
            // var facingLeft = Math.Abs(_currentRotation.y - 180) < 0f;
            // if (rawInput.x > 0 && facingLeft || rawInput.x < 0 && !facingLeft)
            if (rawInput.x > 0 && (int)_currentRotation.y == 180 || rawInput.x < 0 && (int)_currentRotation.y != 180)
                _currentRotation.y = rigidbody.velocity.x > 0 ? 0 : 180;
            
            // Jump pressed and grounded?
            if (!_jumpAction.WasPressedThisFrame() || !Grounded) return;
            
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Jump") || stateInfo.IsName("Fall")) return;
            
            previousSpeed = rigidbody.velocity.x;
            animator.SetTrigger(JumpID);
        }

        private void LateUpdate()
        {
            animator.SetFloat(MoveID, Mathf.Abs(smoothInput.x));
            animator.SetFloat(XVelocityID, Mathf.Abs(rigidbody.velocity.x));
            animator.SetFloat(YVelocityID, rigidbody.velocity.y);
            animator.SetBool(GroundedID, Grounded);
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
