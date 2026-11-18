using UnityEngine;

public class PickpocketerScript : Enemy
{
    [SerializeField] private BasicPathFinding pathFinding;
    [SerializeField] private float enemySpeed = 5f;

    private void Start()
    {
        ChangeState(EnemyState.Chasing);
        pathFinding.EnemySpeedSetUp(enemySpeed);
    }
    private void FixedUpdate()
    {
        if(currentState == EnemyState.Chasing)
            pathFinding.MoveTowardWayPoint();
    }

    protected override void EnemyIdle()
    {
        pathFinding.EnemySpeedSetUp(0);
    }



    // Enemy Hit Player


    protected override void EnemyHitPlayer()
    {
        base.EnemyHitPlayer();
        Debug.Log("Pickpocketer HIHIHI ");
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
