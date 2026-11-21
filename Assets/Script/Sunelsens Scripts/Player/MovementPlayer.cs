using System;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    private float _currentSpeed;
    private Rigidbody2D _rb;
    private Vector2 _direction = Vector2.zero;
    private PlayerState _currentState;

    public event Action OnMove;

    [SerializeField] private ControllerPlayer _controllerPlayer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _controllerPlayer.OnInitialize += Initialize;
        _controllerPlayer.OnSpeedChanged += SetSpeed;
        _controllerPlayer.OnStateChanged += UpdatePlayerState;
    }

    private void OnDisable()
    {
        _controllerPlayer.OnInitialize -= Initialize;
        _controllerPlayer.OnSpeedChanged -= SetSpeed;
        _controllerPlayer.OnStateChanged -= UpdatePlayerState;
    }

    private void FixedUpdate()
    {
        if (_currentState == PlayerState.Walking || _currentState == PlayerState.Sprinting)
        {
            _rb.linearVelocity = new Vector2(_direction.x * _currentSpeed, _direction.y * _currentSpeed);
            OnMove?.Invoke();
        }
        else
        {
            _rb.linearVelocity = Vector2.zero;
        }
    }

    private void Initialize(StatPlayer statPlayer)
    {
        SetSpeed(statPlayer.WalkSpeed);
    }

    private void UpdatePlayerState(PlayerState currPlayerState)
    {
        _currentState = currPlayerState;
    }

    private void SetSpeed(float speed)
    {
        _currentSpeed = speed;
    }

    public void Movement(Vector2 direction)
    {
        _direction = direction;
        
    }
}
