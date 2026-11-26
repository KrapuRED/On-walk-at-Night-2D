using UnityEngine;

public enum EnemyState
{
    Idle,
    Chasing
}


public class Enemy : MonoBehaviour
{

    [SerializeField] protected EnemyState currentState;

    [Header("Enemy Movement")]
    [SerializeField] protected MovementEnemy _enemyMovement;
    [SerializeField] protected float _enemySpeed = 5f;

    [Header("Sprite")]
    [SerializeField] protected SpriteRenderer _enemySpriteRenderer;
    protected bool _startFlip;

    [Header("Animator")]
    [SerializeField] protected Animator _animator;


    protected virtual void Start()
    {
        _startFlip = _enemySpriteRenderer.flipX;
        if (_enemyMovement == null)
        {
            _enemyMovement = GetComponent<MovementEnemy>();
        }

        if (_enemyMovement != null)
        {
            _enemyMovement.SetEnemySpeed(_enemySpeed);
        }
    }

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



    protected virtual void Update()
    {
        if (_enemyMovement != null && (_enemyMovement.Direction.x < 0))
        {
            _enemySpriteRenderer.flipX = !_startFlip;
        }
        else if(_enemyMovement != null)
        {
            _enemySpriteRenderer.flipX = _startFlip;
        }
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
        GameManager.Instance.Defeat();
    }
}
