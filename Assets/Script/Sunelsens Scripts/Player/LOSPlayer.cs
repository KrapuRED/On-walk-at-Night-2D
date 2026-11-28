using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LOSPlayer : MonoBehaviour
{
    [SerializeField] private ControllerPlayer _controllerPlayer;
    private Vector2 _direction;

    [Header("Line Of Sight Setting")]
    [SerializeField] private float _sightRadius;
    [SerializeField] private float _sightAngle = 45;
    [SerializeField] private float _rotationAngle = 90;
    [SerializeField] private int _rayCastCount = 16;
    [SerializeField] private float _rotationSpeed = 2f;

    [Header("Light Setting")]
    [SerializeField] private Transform _lightTransform;
    [SerializeField] private Light2D _sightLight;

    [Header("Layer Mask")]
    [SerializeField] private LayerMask _enemyLayer;

    StalkerScript _stalkerScript;

    private void OnEnable()
    {
        _controllerPlayer.OnLastDirectionChanged += ChangeLightDirection;
    }

    private void OnDisable()
    {
        _controllerPlayer.OnLastDirectionChanged -= ChangeLightDirection;
    }

    private void Update()
    {
        CastRays();
        float targetAngle = Vector2.SignedAngle(Vector2.up, _direction);
        _rotationAngle = Mathf.MoveTowardsAngle(_rotationAngle, targetAngle, _rotationSpeed * Time.deltaTime);
    }

    private void ChangeLightDirection(Vector2 dir)
    {
        _direction = dir;
    }

    private void CastRays()
    {
        bool enemySpotted = false;

        // 1. Tembak GARIS UTAMA (tengah) terlebih dahulu
        Vector2 centerDir = Quaternion.Euler(0, 0, _rotationAngle) * Vector2.up;
        LightRotation();

        if (CheckRay(centerDir))
        {
            enemySpotted = true;
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
                    enemySpotted = true;
                    break;
                }

                // Garis Bawah (- currentOffset dari garis utama)
                Vector2 lowerDir = Quaternion.Euler(0, 0, _rotationAngle - currentOffset) * Vector2.up;
                if (CheckRay(lowerDir))
                {
                    enemySpotted = true;
                    break;
                }
            }
        }

        if (enemySpotted)
        {
            _stalkerScript?.PlayerLooking();
        }
    }

    private bool CheckRay(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _sightRadius, _enemyLayer);
        if (hit.collider == null)
        {
            _stalkerScript = null;
            return false;
        }
        if (hit.collider.TryGetComponent<StalkerScript>(out StalkerScript stalker))
        {
            _stalkerScript = stalker;
            //stalker.PlayerLooking();
            return true;
        }
        return false;
    }

    private void LightRotation()
    {
        _lightTransform.rotation = Quaternion.Euler(0f, 0f, _rotationAngle);
    }

    private void OnDrawGizmos()
    {
        if (_sightRadius <= 0) return;

        // 1. Garis Utama (Tengah) - Merah
        Gizmos.color = Color.red;
        Vector2 centerDir = Quaternion.Euler(0, 0, _rotationAngle) * Vector2.up;
        Gizmos.DrawRay(transform.position, centerDir * _sightRadius);

        // 2. Garis Samping (Kipas Atas & Bawah) - Kuning
        Gizmos.color = Color.yellow;
        int halfRays = _rayCastCount / 2;
        if (halfRays <= 0) return;

        float angleStep = (_sightAngle / 2f) / halfRays;

        for (int i = 1; i <= halfRays; i++)
        {
            float currentOffset = i * angleStep;

            // Garis Atas
            Vector2 upperDir = Quaternion.Euler(0, 0, _rotationAngle + currentOffset) * Vector2.up;
            Gizmos.DrawRay(transform.position, upperDir * _sightRadius);

            // Garis Bawah
            Vector2 lowerDir = Quaternion.Euler(0, 0, _rotationAngle - currentOffset) * Vector2.up;
            Gizmos.DrawRay(transform.position, lowerDir * _sightRadius);
        }

        // 3. Busur Terluar (Batas Radius) - Putih Transparan
        Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, _sightRadius);
    }

}
