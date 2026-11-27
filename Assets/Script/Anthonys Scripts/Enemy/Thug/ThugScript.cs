using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;


[System.Serializable]
public enum ThugState
{
    GoToSound,
    ChasingPlayer
}


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

    [Header("NavMesh Setting")]
    [SerializeField] private NavMeshAgent _agent;

    [Header("Direction")]
    [SerializeField] private Vector2 _targetLocation;

    [Header("SFX Setting")]
    [SerializeField] private float _soundTimer = 1.5f;
    private float _currSoundTimer = 0;

    protected override void Start()
    {
        base.Start();

        _currSoundTimer = _soundTimer;

        _enemyTransform = GetComponent<Transform>();

        if(_agent == null)
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        _playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    protected override void Update()
    {
        CastRays();
        SpriteRotationUpdate();
        PlaySFX();

        if (currentState == EnemyState.Chasing)
        {
            StopPatrol();

            Vector2 directionToPlayer = (_targetLocation - (Vector2)_enemyTransform.position).normalized;
            float targetAngle = Vector2.SignedAngle(Vector2.up, directionToPlayer);
            _rotationAngle = Mathf.MoveTowardsAngle(_rotationAngle, targetAngle, _rotationSpeed * Time.deltaTime);
            _agent.SetDestination(_targetLocation);
            _enemyMovement.SetDirection(_agent.desiredVelocity);
            _agent.nextPosition = _enemyTransform.position;


            // ===== For Check Sound Event ======

            if (_checkingSound)
            {
                if (Vector2.Distance(_targetLocation, _enemyTransform.position) < 0.5f)
                {
                    EnemyDoneCheckingSourceSound();
                }

                return;
            }



        }
        else
        {
            _agent.nextPosition = _enemyTransform.position;
            // ===== For Rotation When Idle ======

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

    }


    private void PlaySFX()
    {
        if (_currSoundTimer < _soundTimer)
        {
            _currSoundTimer += Time.deltaTime;
        }
        else
        {

            if (AudioManager.instance != null && (currentState == EnemyState.Chasing))
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.ThugAlerted_SFX);
                _currSoundTimer = 0;
            }
        }
    }

    // =================================== PATROLLING LEFT RIGHT ===========================================

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


    // =================================== RAYCAST ===========================================
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
            _checkingSound = false;
            _targetLocation = _playerLocation.position;
        }
        else if(!playerSpotted && !_checkingSound)
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
    public void EnemyCheckSourceSound(Vector2 locSound)
    {
        _checkingSound = true;
        ChangeState(EnemyState.Chasing);
        _targetLocation = locSound;
    }

    protected void EnemyDoneCheckingSourceSound()
    {
        _checkingSound = false;
        ChangeState(EnemyState.Idle);
    }



    // =================================== STATE ===========================================
    protected override void EnemyIdle()
    {
        base.EnemyIdle();
        _enemyMovement.SetDirection(Vector3.zero);
        _animator.SetBool("Chasing", false);
    }
    protected override void EnemyChasing()
    {
        StopCoroutine(RotationPatrolCoroutine());
        _patrolCoroutine = null;
        _animator.SetBool("Chasing", true);

    }


    public void SpriteRotationUpdate()
    {
        if (_enemySpriteRenderer == null) return;

        Vector2 lookDir = Quaternion.Euler(0, 0, _rotationAngle) * Vector2.up;
        if (lookDir.x < -0.01f)
        {
            _enemySpriteRenderer.flipX = !_startFlip;
        }
        else if (lookDir.x > 0.01f)
        {
            _enemySpriteRenderer.flipX = _startFlip;
        }
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
