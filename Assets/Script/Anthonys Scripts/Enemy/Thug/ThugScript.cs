using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ThugScript : Enemy
{
    [Header("Line Of Sight Setting")]
    [SerializeField] private float _sightRadius;
    [SerializeField] private Transform _enemyTransform;
    [SerializeField] private Transform _playerLocation;
    [SerializeField] private LayerMask _playerLayer;

    [Header("NavMesh Setting")]
    [SerializeField] private NavMeshAgent _agent;

    private void Start()
    {
        _enemyTransform = GetComponent<Transform>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void Update()
    {
        Collider2D player = Physics2D.OverlapCircle(_enemyTransform.position, _sightRadius, _playerLayer);
        if(player != null)
        {
            if (_playerLocation == null)
            {
                _playerLocation = player.GetComponent<Transform>();
            }
            ChangeState(EnemyState.Chasing);
        }
        else
        {
            ChangeState(EnemyState.Idle);
        }

        if(currentState == EnemyState.Chasing)
        {
            _agent.SetDestination(_playerLocation.position);
        }

    }

    protected override void EnemyIdle()
    {
        base.EnemyIdle();
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
    }
    protected override void EnemyChasing()
    {
        _agent.isStopped = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_enemyTransform.position, _sightRadius);
    }
}
