using UnityEngine;

public enum EnemyState
{
    Idle,
    Chasing
}


public class Enemy : MonoBehaviour
{

    [SerializeField] protected EnemyState currentState;

    public void ChangeState(EnemyState state)
    {
        if(currentState == state) return;
        currentState = state;
        CheckState();
    }

    private void CheckState()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                EnemyIdle();
                break;
            case EnemyState.Chasing:
                EnemyChasing();
                break;
        }
    }
    protected virtual void EnemyIdle()
    {
        Debug.Log("Enemy Idle");
    }
    protected virtual void EnemyChasing()
    {
        Debug.Log("Enemy Chasing");
    }






    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            EnemyHitPlayer();
        }
    }
    
    protected virtual void EnemyHitPlayer()
    {
        Debug.Log("Player Kena Hit");
    }
}
