using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    private float _currentStamina;
    private float _currentSpeed;
    private Rigidbody2D _rb;
    private Vector2 _direction = Vector2.zero;
    private bool _canMove = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!_canMove)
        {
            return;
        }
        _rb.linearVelocity = new Vector2(_direction.x * _currentSpeed, _direction.y * _currentSpeed);
    }

    public void SetSpeed(float speed)
    {
        _currentSpeed = speed;
    }

    public void Movement(Vector2 direction)
    {
        _direction = direction;
    }
}
