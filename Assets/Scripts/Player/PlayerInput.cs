using System.Collections;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private float cleanInputTime;
        public float deadZone;
        [SerializeField] private float smoothTime;
        public float RawAxis { get; private set; }
        public float SmoothAxis { get; private set; }

        private Player _player;
        private PlayerInputActions _actions;
        private InputCommand _lastInputCommand;
        public InputCommand LastInputCommand
        {
            get => _lastInputCommand;
            private set
            {
                _lastInputCommand = value;
                StartCoroutine(CleanLastInput());
            }
        }

        private float _velocity;

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
            _actions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _actions.Ground.Enable();
            _actions.Ground.Attack.performed += OnAttackPerformed;
            _actions.Ground.Jump.performed += OnJumpPerformed;
        }

        private void Update()
        {
            RawAxis = _actions.Ground.Move.ReadValue<float>();
            SmoothAxis = Mathf.SmoothDamp(SmoothAxis, RawAxis, ref _velocity, smoothTime);
            if (Mathf.Abs(SmoothAxis) <= deadZone) SmoothAxis = 0f;
        }

        private IEnumerator CleanLastInput()
        {
            yield return new WaitForSeconds(cleanInputTime);
            _lastInputCommand = InputCommand.None;
        }

        public void CleanLastInputCommand() => _lastInputCommand = InputCommand.None;

        private void OnAttackPerformed(InputAction.CallbackContext obj)
        {
            LastInputCommand = InputCommand.Attack;
            _player.CurrentState.ReadInput(obj, InputCommand.Attack);
        }

        private void OnJumpPerformed(InputAction.CallbackContext obj)
        {
            if (!_player.Grounded) return;
            _player.previousSpeed = _player.Rigidbody.velocity.x;
            LastInputCommand = InputCommand.Jump;
            _player.CurrentState.ReadInput(obj, InputCommand.Jump);
        }

        private void OnDisable()
        {
            _actions.Ground.Disable();
            _actions.Ground.Attack.performed -= OnAttackPerformed;
            _actions.Ground.Jump.performed -= OnJumpPerformed;
        }
    }

    public enum InputCommand
    {
        None,
        Attack,
        Jump
    }
}
