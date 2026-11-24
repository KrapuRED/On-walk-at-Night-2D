using UnityEngine;

public class CarScript : Enemy
{
    [SerializeField] private WaypointMoverEnemy _waypointMoverEnemy;

    private void Start()
    {
        if (_waypointMoverEnemy == null)
        {
            _waypointMoverEnemy = GetComponent<WaypointMoverEnemy>();
        }
        ChangeState(EnemyState.Chasing);
    }
    protected override void EnemyChasing()
    {
        _enemyMovement.SetEnemySpeed(_enemySpeed);
    }
    protected override void EnemyHitPlayer()
    {
        base.EnemyHitPlayer();
        Debug.Log("mOBIL");
    }

    private void FixedUpdate()
    {
        if(currentState == EnemyState.Chasing)
        {
            _enemyMovement.SetDirection(_waypointMoverEnemy.MoveTowardWayPoint());
        }
    }
}
