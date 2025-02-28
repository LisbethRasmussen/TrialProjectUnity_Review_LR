using UnityEngine;
using UnityEngine.InputSystem;

public class IsometricMovementController : MonoBehaviour
{
    [Header("Speed & forces")]
    [Tooltip("Prevents speeding up if the player is inputing two different directions.")]
    [Range(0f, 10f)]
    [SerializeField] private bool _normalizeSpeed;
    [Tooltip("Units per seconds")]
    [SerializeField] private float _topSpeed = 2f;
    [SerializeField] private float _acceleration = 6f;
    [SerializeField] private float _deceleration = 5f;

    [Header("Isometric Movement")]
    [Tooltip("In how much degrees the axis are titled. 0 is for classic orthogonal movement, 45 is default for most isometric games.")]
    [Range(0f, 90f)]
    [SerializeField] private float _tiltMovementFactor = 45;

    [Header("Input References")]
    [SerializeField] private InputActionReference _movementInputReference;

    private Rigidbody2D _rigidbody;
    private Vector2 _currentInput;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    #region Input Management

    private void OnEnable()
    {
        _movementInputReference.action.Enable();
        _movementInputReference.action.performed += OnInput;
        _movementInputReference.action.canceled += OnInput;
    }

    private void OnDisable()
    {
        _movementInputReference.action.Disable();
        _movementInputReference.action.performed -= OnInput;
        _movementInputReference.action.canceled -= OnInput;
    }

    private void OnInput(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            _currentInput = ctx.ReadValue<Vector2>();
        }
        else
        {
            _currentInput = Vector2.zero;
        }
    }

    #endregion

    private void FixedUpdate()
    {
        ManageMovement();
    }

    private void ManageMovement()
    {
        if (_currentInput != Vector2.zero)
        {
            // Normalize the input if needed
            Vector2 movementInput = _normalizeSpeed ? _currentInput.normalized : _currentInput;

            // Tilt the axis to the desired angle
            Vector2 tiltedInput = TiltVector(movementInput, _tiltMovementFactor);

            // Calculate the target velocity and acceleration
            Vector2 targetVelocity = tiltedInput * _topSpeed;
            Vector2 acceleration = (targetVelocity - _rigidbody.linearVelocity) * _acceleration;

            _rigidbody.AddForce(acceleration, ForceMode2D.Force);
        }
        else
        {
            Vector2 deceleration = -_rigidbody.linearVelocity.normalized * _deceleration;
            _rigidbody.AddForce(deceleration, ForceMode2D.Force);
        }
    }

    static private Vector2 TiltVector(Vector2 vector, float angle)
    {
        return Quaternion.Euler(0, 0, -angle) * vector;
    }
}
