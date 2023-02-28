using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float dequeueTime;
        [field: SerializeField] public float DeadZone { get; private set; }
        [SerializeField] private float smoothTime;
        public float RawAxis { get; private set; }
        public float SmoothAxis { get; private set; }

        private Player _player;
        private PlayerInputActions _actions;

        private Queue<InputCommand> _buffer;

        private float _velocity;

        #endregion

        #region Unity API

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
            _actions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _actions.Enable();
            _actions.Ground.Attack.performed += OnAttackPerformed;
            _actions.Ground.Jump.performed += OnJumpPerformed;
        }

        private void OnDisable()
        {
            _actions.Disable();
            _actions.Ground.Attack.performed -= OnAttackPerformed;
            _actions.Ground.Jump.performed -= OnJumpPerformed;
        }

        private void Update()
        {
            RawAxis = _actions.Ground.Move.ReadValue<float>();
            SmoothAxis = Mathf.SmoothDamp(SmoothAxis, RawAxis, ref _velocity, smoothTime);
            if (Mathf.Abs(SmoothAxis) <= DeadZone) SmoothAxis = 0f;
        }

        #endregion

        #region Event functions

        private void OnAttackPerformed(InputAction.CallbackContext obj)
        {
            EnqueueCommand(InputCommand.Attack);
            _player.CurrentState.ReadInput(obj, InputCommand.Attack);
        }

        private void OnJumpPerformed(InputAction.CallbackContext obj)
        {
            if (!_player.Grounded) return;
            EnqueueCommand(InputCommand.Jump);
            _player.previousSpeed = _player.Rigidbody.velocity.x;
            _player.CurrentState.ReadInput(obj, InputCommand.Jump);
        }

        #endregion

        #region Buffer

        public InputCommand PeekCommand() => _buffer.Count == 0 ? InputCommand.None : _buffer.Dequeue();

        private void EnqueueCommand(InputCommand command)
        {
            _buffer.Enqueue(command);
            StartCoroutine(DequeueCommand());
        }

        private IEnumerator DequeueCommand()
        {
            yield return new WaitForSeconds(dequeueTime);
            if (_buffer.Count > 0)
                _buffer.Dequeue();
        }

        #endregion
    }

    public enum InputCommand
    {
        None,
        Attack,
        Jump
    }
}
