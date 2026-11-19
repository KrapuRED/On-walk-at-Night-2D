using UnityEngine;

public class ControllerPlayer : StatePlayer
{
    private InputPlayer _inputPlayer;
    private MovementPlayer _movementPlayer;

    [SerializeField] private float _speed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _sprintCost;

    [SerializeField] private float _maxStamina;
    [SerializeField] private float _staminaRegen;

    private Vector2 _currentDirection = Vector2.zero;
    private Vector2 _lastDirection = Vector2.right;
    private bool _canSprint = false;
    private void Awake()
    {
        _inputPlayer = GetComponent<InputPlayer>();
        _movementPlayer = GetComponent<MovementPlayer>();

        _movementPlayer.SetSpeed(_speed);
    }

    private void OnEnable()
    {
        _inputPlayer.OnMovementInput += Movement;
        _inputPlayer.OnSprintInput += Sprint;
    }

    private void Movement(Vector2 direction)
    {
        Vector2 normalizedDirection = direction.normalized;
        _movementPlayer.Movement(normalizedDirection);
        _currentDirection = normalizedDirection;
        if (_currentDirection != Vector2.zero)
        {
            _lastDirection = _currentDirection;
        }
        UpdateMovementState();
    }

    private void UpdateMovementState()
    {
        if (_currentDirection == Vector2.zero)
        {
            ChangeState(PlayerState.Idle);
            return;
        }

        if (_canSprint)
        {
            ChangeState(PlayerState.Sprinting);
        }
        else
        {
            ChangeState(PlayerState.Walking);
        }
    }

    private void Sprint(bool canSprint)
    {
        _canSprint = canSprint;
        UpdateMovementState();
    }

    protected override void PlayerWalking()
    {
        base.PlayerWalking();
        _movementPlayer.SetSpeed(_speed);
    }

    protected override void PlayerSprinting()
    {
        base.PlayerSprinting();
        _movementPlayer.SetSpeed(_sprintSpeed);
    }
}
