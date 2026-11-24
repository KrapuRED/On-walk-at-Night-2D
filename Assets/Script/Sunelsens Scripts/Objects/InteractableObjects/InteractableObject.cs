using UnityEngine;
using UnityEngine.UI;

public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] protected CanvasGroup _canvasGroup;
    [SerializeField] protected float _interactTime;
    [SerializeField] protected Image _notificationImage;
    [SerializeField] protected float _soundRadius;
    protected SpriteRenderer _spriteRenderer;
    protected Collider2D[] _soundRaycast;
    public float InteractTime => _interactTime;
    public bool IsInteracted { get; protected set; }
    protected bool _isInteracting = false;
    public virtual bool CanInteract()
    {
        return !IsInteracted;
    }

    public virtual void Interact()
    {
        if (!CanInteract())
        {
            return;
        }
        SendSound();
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnFocus()
    {
        Debug.Log("On Focus");
        ShowHideIcon(true);
    }

    public void OffFocus()
    {
        Debug.Log("Off Focus");
        ShowHideIcon(false);
    }

    protected void ShowHideIcon(bool showIcon)
    {
        if (showIcon)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        else
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }

    public void UpdateNotification(float currentInteractTime)
    {
        if (currentInteractTime <= 0.02)
        {
            _isInteracting = false;
        }
        else
        {
            _isInteracting = true;
        }
        _notificationImage.fillAmount = Mathf.Clamp01(currentInteractTime / _interactTime);
    }

    protected void SendSound()
    {
        Debug.Log("Sound Sent");
        _soundRaycast = Physics2D.OverlapCircleAll(transform.position, _soundRadius);

        foreach (Collider2D col in _soundRaycast)
        {
            col.TryGetComponent<ThugScript>(out ThugScript thug);
            if (thug != null)
            {
                thug.EnemyCheckSourceSound(transform.position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _soundRadius);
    }
}
