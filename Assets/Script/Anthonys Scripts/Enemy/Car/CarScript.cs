using UnityEngine;

public class CarScript : Enemy
{
    [SerializeField] private BasicPathFinding pathFinding;
    [SerializeField] private float enemySpeed = 5f;

    private void Start()
    {
        pathFinding.EnemySpeedSetUp(enemySpeed);
    }
    private void FixedUpdate()
    {
        pathFinding.MoveTowardWayPoint();
    }




    protected override void EnemyHitPlayer()
    {
        base.EnemyHitPlayer();
        Debug.Log("mOBIL");
    }
}
