using UnityEngine;

public class PickpocketerScript : Enemy
{
    [SerializeField] private WaypointMoverEnemy _waypointMoverEnemy;
    protected override void Start()
    {
        base.Start();
        if (_waypointMoverEnemy == null)
        {
            _waypointMoverEnemy = GetComponent<WaypointMoverEnemy>();
        }
        ChangeState(EnemyState.Chasing);
    }

    protected override void EnemyIdle()
    {
        _enemyMovement.SetEnemySpeed(0);
    }

    protected override void EnemyChasing()
    {
        _enemyMovement.SetEnemySpeed(_enemySpeed);
    }  

    protected override void EnemyHitPlayer()
    {
        base.EnemyHitPlayer();
        Debug.Log("Pickpocketer HIHIHI ");
    }
    private void FixedUpdate()
    {
        if (currentState == EnemyState.Chasing)
        {
            _enemyMovement.SetDirection(_waypointMoverEnemy.MoveTowardWayPoint());
        }
    }


    // Enemy Hit Vending Machine

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision == null) return;

        if (collision.gameObject.CompareTag("Mesin"))
        {
            ChangeState(EnemyState.Idle);
        }
    }
}
