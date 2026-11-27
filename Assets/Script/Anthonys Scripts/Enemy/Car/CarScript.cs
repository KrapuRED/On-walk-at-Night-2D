using UnityEngine;

public class CarScript : Enemy
{
    [SerializeField] private WaypointMoverEnemy _waypointMoverEnemy;

    [Header("Audio")]
    [SerializeField] private float _maxDistanceSound = 3f;
    [SerializeField] private AudioSource carSource;
    private float _audioTime;
    private float _timer;

    private Transform _playerTransform;
    protected override void Start()
    {
        base.Start();

        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        _audioTime = AudioManager.instance.CarWalking_SFX.length;

        PlayAudio();

        if (_waypointMoverEnemy == null)
        {
            _waypointMoverEnemy = GetComponent<WaypointMoverEnemy>();
        }
        ChangeState(EnemyState.Chasing);
    }

    protected override void Update()
    {
        base.Update();

        //Distance Audio
        float distance = Vector2.Distance(_playerTransform.position, transform.position);
        float value = 1f - Mathf.Clamp01(distance / _maxDistanceSound);
        carSource.volume = value;
    }

    private void PlayAudio()
    {

        carSource.clip = AudioManager.instance.CarWalking_SFX;
        carSource.loop = true;
        carSource.Play();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.burlywood;
        Gizmos.DrawWireSphere(transform.position, _maxDistanceSound);
    }

}
