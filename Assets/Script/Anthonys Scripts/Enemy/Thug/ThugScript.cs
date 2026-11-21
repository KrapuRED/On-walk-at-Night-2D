using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ThugScript : Enemy
{
    [Header("Line Of Sight Setting")]
    [SerializeField] private float _sightRadius;
    [SerializeField] private float _sightAngle = 45;
    //[SerializeField] private float _rotationAngle = 45;
    [SerializeField] private Transform _enemyTransform;
    [SerializeField] private Transform _playerLocation;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private bool _thugPatrol = false;

    [Header("Raycast LOS Setting")]
    [SerializeField] private int _rayCastCount = 16;


    [Header("NavMesh Setting")]
    [SerializeField] private NavMeshAgent _agent;

    private void Start()
    {
        _enemyTransform = GetComponent<Transform>();
        _agent = GetComponent<NavMeshAgent>();
        _playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void Update()
    {
        CastRays();

        if (currentState == EnemyState.Chasing)
        {
            _agent.SetDestination(_playerLocation.position);
        }

    }

    private void CastRays()
    {
        float angleStep = _sightAngle / _rayCastCount;
        bool playerSpotted = false;

        for (int i = 0; i < _rayCastCount; i++)
        {
            float angle = i * angleStep;

            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.up;
            RaycastHit2D hit = Physics2D.Raycast(_enemyTransform.position, dir, _sightRadius, _playerLayer);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                playerSpotted = true;
                break;
            }
        }

        if (playerSpotted)
        {
            ChangeState(EnemyState.Chasing);
        }
        else
        {
            ChangeState(EnemyState.Idle);
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_enemyTransform.position, _sightRadius);

        float angleStep = _sightAngle / _rayCastCount;

        for (int i = 0; i < _rayCastCount; i++)
        {
            float angle = i * angleStep;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;

            Gizmos.DrawRay(transform.position, direction * _sightRadius);
        }
    }
}
