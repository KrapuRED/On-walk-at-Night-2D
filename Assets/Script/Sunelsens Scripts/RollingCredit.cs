
using DG.Tweening;
using UnityEngine;

public class RollingCredit : MonoBehaviour
{
    [SerializeField] private float _scrollDuration = 50f;
    [SerializeField] private float _endPosition = 1000f;

    private RectTransform _rectTransform;
    private TransitionManager _transitionManager;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _transitionManager = TransitionManager.Instance;
    }

    private void Start()
    {
        _rectTransform.DOAnchorPosY(_endPosition, _scrollDuration).SetEase(Ease.Linear).OnComplete(() => 
        {
            _transitionManager.LoadScene("MainMenu"); 
        });
    }
}
