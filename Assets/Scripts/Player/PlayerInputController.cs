using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        public float deadZone;
        [SerializeField] private float smoothTime;
        public float RawAxis { get; private set; }
        public float SmoothAxis { get; private set; }
        
        private PlayerController _player;
        private KnightActions _actions;

        private float _velocity;

        private void Awake()
        {
            _player = GetComponentInParent<PlayerController>();
            _actions = new KnightActions();
        }

        private void Update()
        {
            RawAxis = _actions.Player.Move.ReadValue<float>();
            SmoothAxis = Mathf.SmoothDamp(SmoothAxis, RawAxis, ref _velocity, smoothTime);
            if (Mathf.Abs(SmoothAxis) <= deadZone) SmoothAxis = 0f;
        }

        private void OnAttackPerformed(InputAction.CallbackContext obj)
        {
            _player.CurrentState.ReadInput(obj, InputCommand.Attack);
        }

        private void OnJumpPerformed(InputAction.CallbackContext obj)
        {
            if (!_player.Grounded) return;
            _player.previousSpeed = _player.rigidbody.velocity.x;
            _player.CurrentState.ReadInput(obj, InputCommand.Jump);
        }

        private void OnEnable()
        {
            _actions.Enable();
            _actions.Player.Attack.performed += OnAttackPerformed;
            _actions.Player.Jump.performed += OnJumpPerformed;
        }

        private void OnDisable()
        {
            _actions.Disable();
            _actions.Player.Attack.performed -= OnAttackPerformed;
            _actions.Player.Jump.performed -= OnJumpPerformed;
        }
    }

    public enum InputCommand
    {
        Attack,
        Jump
    }
}
