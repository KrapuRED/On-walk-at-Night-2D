using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    private float _currentSpeed;
    private Rigidbody2D _rb;
    private Vector2 _direction = Vector2.zero;
    private bool _canMove = true;

    [SerializeField] private ControllerPlayer _controllerPlayer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _controllerPlayer.OnInitialize += Initialize;
        _controllerPlayer.OnSprint += Sprint;
        _controllerPlayer.OnWalk += Walk;
    }

    private void OnDisable()
    {
        _controllerPlayer.OnInitialize -= Initialize;
        _controllerPlayer.OnSprint -= Sprint;
        _controllerPlayer.OnWalk -= Walk;
    }

    private void FixedUpdate()
    {
        if (!_canMove)
        {
            return;
        }
        _rb.linearVelocity = new Vector2(_direction.x * _currentSpeed, _direction.y * _currentSpeed);
    }

    private void Initialize(StatPlayer statPlayer)
    {
        SetSpeed(statPlayer.WalkSpeed);
    }

    private void SetSpeed(float speed)
    {
        _currentSpeed = speed;
    }

    public void Movement(Vector2 direction)
    {
        _direction = direction;
    }

    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
    }

    private void Sprint(float speed, float notUsed)
    {
        SetSpeed(speed);
    }

    private void Walk(float speed)
    {
        SetSpeed(speed);
    }
}
