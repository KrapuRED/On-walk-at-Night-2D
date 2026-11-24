using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class ThugScript : Enemy
{
    [Header("Line Of Sight Setting")]
    [SerializeField] private float _sightRadius;
    [SerializeField] private float _sightAngle = 45;
    [SerializeField] private float _rotationAngle = 90;
    [SerializeField] private float _rotationSpeed = 2f;
    [SerializeField] private Transform _enemyTransform;
    [SerializeField] private Transform _playerLocation;
    [SerializeField] private LayerMask _playerLayer;

    [Header("Light Setting")]
    [SerializeField] private Transform _lightTransform;
    [SerializeField] private Light2D _sightLight;

    [Header("Patrol Setting")]
    [SerializeField] private bool _thugPatrol = false;
    [SerializeField] private float _rotationPatrol = 90;
    [SerializeField] private float _patrolWaitTime = 1f;
    private Coroutine _patrolCoroutine;

    [Header("Raycast LOS Setting")]
    [SerializeField] private int _rayCastCount = 16;

    [Header("Event Setting")]
    [SerializeField] private bool _checkingSound = false;
    [SerializeField] private Vector2 _sourceSoundLocation;

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

        if (_checkingSound)
        {
            Vector2 dir = _agent.velocity.normalized;

            StopPatrol();

            float targetAngle = Vector2.SignedAngle(Vector2.up, dir);
            _rotationAngle = Mathf.MoveTowardsAngle(_rotationAngle, targetAngle, _rotationSpeed * Time.deltaTime);
            _agent.SetDestination(_sourceSoundLocation);

            if(Vector2.Distance(_sourceSoundLocation, _enemyTransform.position) < 0.5f)
            {
                EnemyDoneCheckingSourceSound();
            }

            return;
        }

        if (currentState == EnemyState.Idle)
        {
            if (_thugPatrol)
            {
                if (_patrolCoroutine == null)
                {
                    _patrolCoroutine = StartCoroutine(RotationPatrolCoroutine());
                }
            }
            else
            {
                StopPatrol();
            }
        }
        else
        {
            StopPatrol();
            Vector2 directionToPlayer = (_playerLocation.position - _enemyTransform.position).normalized;
            float targetAngle = Vector2.SignedAngle(Vector2.up, directionToPlayer);

            _rotationAngle = Mathf.MoveTowardsAngle(_rotationAngle, targetAngle, _rotationSpeed * Time.deltaTime);

            _agent.SetDestination(_playerLocation.position);
        }

    }


    private IEnumerator RotationPatrolCoroutine()
    {
        float leftTarget = _rotationAngle + _rotationPatrol;
        float rightTarget = _rotationAngle - _rotationPatrol;

        while (true)
        {
            // 1. Putar ke kiri secara linier (tidak memutar balik 360 derajat)
            yield return new WaitUntil(() =>
            {
                _rotationAngle = Mathf.MoveTowards(_rotationAngle, leftTarget, _rotationSpeed * Time.deltaTime);
                return Mathf.Approximately(_rotationAngle, leftTarget);
            });

            yield return new WaitForSeconds(_patrolWaitTime);

            // 2. Putar ke kanan secara linier melewati titik tengah awal
            yield return new WaitUntil(() =>
            {
                _rotationAngle = Mathf.MoveTowards(_rotationAngle, rightTarget, _rotationSpeed * Time.deltaTime);
                return Mathf.Approximately(_rotationAngle, rightTarget);
            });

            yield return new WaitForSeconds(_patrolWaitTime);
        }
    }

    private void StopPatrol()
    {
        if (_patrolCoroutine != null)
        {
            StopCoroutine(_patrolCoroutine);
            _patrolCoroutine = null;
        }
    }

    private void CastRays()
    {
        bool playerSpotted = false;

        // 1. Tembak GARIS UTAMA (tengah) terlebih dahulu
        Vector2 centerDir = Quaternion.Euler(0, 0, _rotationAngle) * Vector2.up;
        LightRotation();

        if (CheckRay(centerDir))
        {
            playerSpotted = true;
        }
        else
        {
            // 2. Tembak raycast ATAS dan BAWAH secara simetris dari garis utama
            int halfRays = _rayCastCount / 2;
            float angleStep = (_sightAngle / 2f) / halfRays;

            for (int i = 1; i <= halfRays; i++)
            {
                float currentOffset = i * angleStep;

                // Garis Atas (+ currentOffset dari garis utama)
                Vector2 upperDir = Quaternion.Euler(0, 0, _rotationAngle + currentOffset) * Vector2.up;
                if (CheckRay(upperDir))
                {
                    playerSpotted = true;
                    break;
                }

                // Garis Bawah (- currentOffset dari garis utama)
                Vector2 lowerDir = Quaternion.Euler(0, 0, _rotationAngle - currentOffset) * Vector2.up;
                if (CheckRay(lowerDir))
                {
                    playerSpotted = true;
                    break;
                }
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


    private bool CheckRay(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(_enemyTransform.position, direction, _sightRadius, _playerLayer);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    private void LightRotation()
    {
        _lightTransform.rotation = Quaternion.Euler(0f, 0f, _rotationAngle);
    }


    // =================================== SOUND EVENT ===========================================
    protected void EnemyCheckSourceSound(Vector2 locSound)
    {
        _checkingSound = true;
        _sourceSoundLocation = locSound;
    }

    protected void EnemyDoneCheckingSourceSound()
    {
        _checkingSound = false;
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
        StopCoroutine(RotationPatrolCoroutine());
        _patrolCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        // Gambar Garis Utama (Warna Merah biar kelihatan bedanya)
        Gizmos.color = Color.red;
        Vector2 centerDir = Quaternion.Euler(0, 0, _rotationAngle) * Vector2.up;
        Gizmos.DrawRay(transform.position, centerDir * _sightRadius);

        // Gambar Garis Atas & Bawah (Warna Kuning)
        Gizmos.color = Color.yellow;
        int halfRays = _rayCastCount / 2;
        if (halfRays <= 0) return;

        float angleStep = (_sightAngle / 2f) / halfRays;

        for (int i = 1; i <= halfRays; i++)
        {
            float currentOffset = i * angleStep;

            Vector2 upperDir = Quaternion.Euler(0, 0, _rotationAngle + currentOffset) * Vector2.up;
            Gizmos.DrawRay(transform.position, upperDir * _sightRadius);

            Vector2 lowerDir = Quaternion.Euler(0, 0, _rotationAngle - currentOffset) * Vector2.up;
            Gizmos.DrawRay(transform.position, lowerDir * _sightRadius);
        }
    }
}
