using UnityEngine;

public class MovementEnemy : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Rigidbody2D rb;
    private Vector2 _direction;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
    public void SetEnemySpeed(float speed)
    {
        movementSpeed = speed;
    }
    private void FixedUpdate()
    {
        Vector2 dir = _direction.normalized;
        rb.linearVelocity = dir * movementSpeed;
    }
}
