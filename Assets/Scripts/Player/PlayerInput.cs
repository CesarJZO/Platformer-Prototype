using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        [field: SerializeField] public float DeadZone { get; private set; }
        [SerializeField] private float smoothTime;

        public float RawAxis { get; private set; }
        public float SmoothAxis { get; private set; }

        private Player _player;
        private PlayerInputActions _actions;

        private float _velocity;

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

        private void Update()
        {
            RawAxis = _actions.Ground.Move.ReadValue<float>();
            SmoothAxis = Mathf.SmoothDamp(SmoothAxis, RawAxis, ref _velocity, smoothTime);
            if (Mathf.Abs(SmoothAxis) <= DeadZone) SmoothAxis = 0f;
        }

        private void OnAttackPerformed(InputAction.CallbackContext obj)
        {
            _player.CurrentState.ReadInput(obj, InputCommand.Attack);
        }

        private void OnJumpPerformed(InputAction.CallbackContext obj)
        {
            if (!_player.Grounded) return;
            _player.previousSpeed = _player.Rigidbody.velocity.x;
            _player.CurrentState.ReadInput(obj, InputCommand.Jump);
        }

        private void OnDisable()
        {
            _actions.Disable();
            _actions.Ground.Attack.performed -= OnAttackPerformed;
            _actions.Ground.Jump.performed -= OnJumpPerformed;
        }
    }

    public enum InputCommand
    {
        Attack,
        Jump
    }
}
