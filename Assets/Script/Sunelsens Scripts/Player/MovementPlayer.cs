using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _sprintCost;

    [SerializeField] private float _maxStamina;
    [SerializeField] private float _staminaRegen;

    private float _currentStamina;
    private float _currentSpeed;
    private Rigidbody2D _rb;
    private Vector2 _direction = Vector2.zero;
    private InputPlayer _inputPlayer;
    private bool _canMove = true;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _inputPlayer = GetComponent<InputPlayer>();

        _currentSpeed = _speed;
        _currentStamina = _maxStamina;
    }

    private void OnEnable()
    {
        _inputPlayer.OnMovementInput += Movement;
        _inputPlayer.OnSprintInput += Sprint;
    }

    private void FixedUpdate()
    {
        if (!_canMove)
        {
            return;
        }
        _rb.linearVelocity = new Vector2(_direction.x * _currentSpeed, _direction.y * _currentSpeed);
    }

    private void Update()
    {
        
    }

    private void Movement(Vector2 direction)
    {
        Vector2 normalizedDirection = direction.normalized;
        _direction = normalizedDirection;
    }

    private void Sprint(bool isSprinting)
    {
        if (isSprinting)
        {
            Debug.Log("Is Sprinting");
            _currentSpeed = _sprintSpeed;
        }
        else
        {
            _currentSpeed = _speed;
        }
    }
}
