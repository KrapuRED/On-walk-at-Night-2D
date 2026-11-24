using System;
using UnityEngine;

public class ControllerPlayer : StatePlayer
{
    private InputPlayer _inputPlayer;
    private MovementPlayer _movementPlayer;
    private StaminaPlayer _staminaPlayer;

    public event Action<StatPlayer> OnInitialize;
    public event Action<float> OnSpeedChanged;
    public event Action<float> OnSprint;

    [SerializeField] private StatPlayer _statPlayer;

    private Vector2 _currentDirection = Vector2.zero;
    private Vector2 _lastDirection = Vector2.right;
    private bool _canSprint = false;
    private float _currStamina;
    private void Awake()
    {
        _inputPlayer = GetComponent<InputPlayer>();
        _movementPlayer = GetComponent<MovementPlayer>();
        _staminaPlayer = GetComponent<StaminaPlayer>();
    }

    private void OnEnable()
    {
        _inputPlayer.OnMovementInput += Movement;
        _inputPlayer.OnSprintInput += Sprint;
        _staminaPlayer.OnStaminaChanged += StaminaCount;
    }

    private void OnDisable()
    {
        _inputPlayer.OnMovementInput -= Movement;
        _inputPlayer.OnSprintInput -= Sprint;
        _staminaPlayer.OnStaminaChanged -= StaminaCount;
    }

    private void Start()
    {
        _currStamina = _statPlayer.MaxStamina;
        OnInitialize?.Invoke(_statPlayer);
    }

    private void Update()
    {
        UpdateMovementState();
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
        
    }

    private void UpdateMovementState()
    {
        if (_currentState == PlayerState.Interacting)
        {
            return;
        }

        if (_currentDirection == Vector2.zero)
        {
            ChangeState(PlayerState.Idle);
            return;
        }

        if (_canSprint && _currStamina > 0)
        {
            ChangeState(PlayerState.Sprinting);
        }
        else
        {
            ChangeState(PlayerState.Walking);
        }
    }

    public PlayerState GetCurrentState()
    {
        return _currentState;
    }

    private void Sprint(bool canSprint)
    {
        if (_currentState == PlayerState.Dead)
        {
            return;
        }
        _canSprint = canSprint;
        UpdateMovementState();
    }

    private void StaminaCount(float currStamina)
    {
        _currStamina = currStamina;
    }

    protected override void PlayerWalking()
    {
        base.PlayerWalking();
        OnSpeedChanged?.Invoke(_statPlayer.WalkSpeed);
    }

    protected override void PlayerSprinting()
    {
        base.PlayerSprinting();
        OnSpeedChanged?.Invoke(_statPlayer.SprintSpeed);
        OnSprint?.Invoke(_statPlayer.SprintCost);
    }

    protected override void PlayerInteracting()
    {
        base.PlayerInteracting();
    }
}
