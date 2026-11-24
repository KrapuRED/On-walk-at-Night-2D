using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _target;
    [SerializeField] private RectTransform _pointerRect;
    [SerializeField] private float _padding;
    [SerializeField] private Sprite _arrowSprite;
    [SerializeField] private Sprite _objectSprite;
    private Image _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        

        Vector2 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(_target.position);

        bool targetIsOffScreen = targetPositionScreenPoint.x <= _padding || targetPositionScreenPoint.y <= _padding || targetPositionScreenPoint.x >= Screen.width - _padding || targetPositionScreenPoint.y >= Screen.height - _padding;

        if (targetIsOffScreen)
        {
            _spriteRenderer.sprite = _arrowSprite;
            SetPointerRotation();
            Vector2 cappedTargetPosition = targetPositionScreenPoint;

            cappedTargetPosition.x = Mathf.Clamp(cappedTargetPosition.x, _padding, Screen.width - _padding);
            cappedTargetPosition.y = Mathf.Clamp(cappedTargetPosition.y, _padding, Screen.height - _padding);

            Vector2 pointerWorldPosition = Camera.main.ScreenToWorldPoint(cappedTargetPosition);
            _pointerRect.position = pointerWorldPosition;
        }
        else
        {
            _pointerRect.transform.rotation = Quaternion.Euler(0, 0, 0);
            _spriteRenderer.sprite = _objectSprite;
            Vector2 pointerWorldPosition = Camera.main.ScreenToWorldPoint(targetPositionScreenPoint);
            _pointerRect.position = pointerWorldPosition;
        }
    }

    private void SetPointerRotation()
    {
        Vector2 direction = _target.position - _player.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _pointerRect.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
