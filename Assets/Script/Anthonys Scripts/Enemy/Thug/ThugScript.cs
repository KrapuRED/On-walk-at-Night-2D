using Unity.VisualScripting;
using UnityEngine;

public class ThugScript : Enemy
{
    [Header("Line Of Sight Setting")]
    [SerializeField] private float _sightRadius;
    [SerializeField] private Transform _enemyTransform;
    [SerializeField] private LayerMask _playerLayer;

    private void Start()
    {
        _enemyTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        Collider2D player = Physics2D.OverlapCircle(_enemyTransform.position, _sightRadius, _playerLayer);
        if(player != null)
        {
            ChangeState(EnemyState.Chasing);
        }
        else
        {
            ChangeState(EnemyState.Idle);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_enemyTransform.position, _sightRadius);
    }
}
