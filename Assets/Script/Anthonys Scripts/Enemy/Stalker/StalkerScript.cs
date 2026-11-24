using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public enum StalkerState
{
    FindPlayer,
    FlankingPlayer,
    ChasingPlayer
}

public class StalkerScript : Enemy
{
    [Header("Setting - Enemy")]
    [SerializeField] private StalkerState currentStalkerState;
    [SerializeField] private Transform _enemyTransform;
    [SerializeField] private float _findPlayerRadius = 5f;
    [SerializeField] private float _chasingRadius = 2.5f;

    [Header("Raycast LOS Setting")]
    [SerializeField] private int _rayCastCount = 16;

    [Header("Setting - Player")]
    [SerializeField] private Transform _playerLocation;
    [SerializeField] private LayerMask _playerLayer;

    [Header("Setting - Player -- BackPlayer")]
    [SerializeField] private Vector2 _playerBackLocation;
    [SerializeField] private float _maxRaycastDistance = 3f;
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private float _distanceCheck = 0.5f;
    [SerializeField] private float _raycastTimer = 3f;
    private Coroutine _findPlayerBackCoroutine;


    [Header("NavMesh Setting")]
    [SerializeField] private NavMeshAgent _agent;

    private void Awake()
    {
        _enemyTransform = GetComponent<Transform>();
        _agent = GetComponent<NavMeshAgent>();
        _playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        ChangeState(EnemyState.Chasing);
    }

    public void ChangeStalkerState(StalkerState state)
    {
        if (currentStalkerState == state) return;
        currentStalkerState = state;
        CheckStalkerState();
    }

    private void CheckStalkerState()
    {
        if(currentState == EnemyState.Idle) return;
        switch (currentStalkerState)
        {
            case StalkerState.FindPlayer:
                break;
        }
    }

    private void Update()
    {
        switch (currentStalkerState)
        {
            case StalkerState.FindPlayer:
                _agent.SetDestination(_playerLocation.position);
                break;
            case StalkerState.FlankingPlayer:
                _agent.SetDestination(_playerBackLocation);

                if (Vector2.Distance(_playerBackLocation, _enemyTransform.position) < _distanceCheck)
                {
                    CalculatePlayerBackLocation();
                    RestartFindPlayerBackCoroutine();
                }

                break;
            case StalkerState.ChasingPlayer:
                _agent.SetDestination(_playerLocation.position);
                break;

        }

        CheckFirstPhase();
        CheckFinalPhase();

    }

    private void CheckFirstPhase()
    {
        if (currentStalkerState == StalkerState.ChasingPlayer) return;

        Collider2D hit = Physics2D.OverlapCircle(_enemyTransform.position, _findPlayerRadius, _playerLayer);
        if (hit == null)
        {
            ChangeStalkerState(StalkerState.FindPlayer);
        }
        else if(hit != null)
        {
            ChangeStalkerState(StalkerState.FlankingPlayer);

            if(_findPlayerBackCoroutine == null)
            {
                _findPlayerBackCoroutine = StartCoroutine(FindPlayerBack());
            }
        }
    }

    // When Enemy Can See Player
    private void CheckFinalPhase()
    {
        bool spotPlayer = CastRays();

        if (spotPlayer)
        {
            ChangeStalkerState(StalkerState.ChasingPlayer);
        }
        else if (currentStalkerState == StalkerState.ChasingPlayer)
        {
            if (_findPlayerBackCoroutine != null)
            {
                StopCoroutine(_findPlayerBackCoroutine);
                _findPlayerBackCoroutine = null;
            }
            ChangeStalkerState(StalkerState.FindPlayer);
        }
    }

    private bool CastRays()
    {
        float angleStep = 360 / _rayCastCount;
        bool playerSpotted = false;

        for (int i = 0; i < _rayCastCount; i++)
        {
            float angle = i * angleStep;

            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.up;
            RaycastHit2D hit = Physics2D.Raycast(_enemyTransform.position, dir, _chasingRadius, _obstacleLayer);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                playerSpotted = true;
                break;
            }
        }

        return playerSpotted;

    }

    private void CalculatePlayerBackLocation()
    {
        Vector2 playerDir = Vector2.right; // Ambil direction dari Script Player
        Vector2 playerBackDir = -playerDir.normalized;
        Vector2 playerPos = _playerLocation.position;

        RaycastHit2D hit = Physics2D.Raycast(playerPos, playerBackDir, _maxRaycastDistance, _obstacleLayer);
        if (hit.collider != null)
        {
            // Gunakan hit.point + sedikit offset menjauh dari tembok agar tidak menempel ke collider
            _playerBackLocation = hit.point + (hit.normal * 0.5f);
        }
        else
        {
            _playerBackLocation = playerPos + (playerBackDir * _maxRaycastDistance);
        }
    }
    public IEnumerator FindPlayerBack()
    {
        while (currentStalkerState == StalkerState.FlankingPlayer)
        {
            CalculatePlayerBackLocation();

            float waitTime = Random.Range(_raycastTimer - 1f, _raycastTimer + 1f);
            yield return new WaitForSeconds(waitTime);
        }

        _findPlayerBackCoroutine = null;
    }


    private void RestartFindPlayerBackCoroutine()
    {
        if (_findPlayerBackCoroutine != null)
        {
            StopCoroutine(_findPlayerBackCoroutine);
        }
        _findPlayerBackCoroutine = StartCoroutine(FindPlayerBack());
    }



    // ======================== ENEMY WHEN SPOTTED ===============================



    //Use At When player Spotted Enemy
    public void PlayerLooking()
    {
        ChangeState(EnemyState.Idle);
    }





    // ======================== ENEMY STATE ===============================

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

}
