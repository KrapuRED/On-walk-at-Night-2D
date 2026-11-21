using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite _knockedDownSprite;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _interactTime;
    [SerializeField] private Image _notificationImage;
    public bool IsDropped {  get; private set; }
    public float InteractTime => _interactTime;
    

    public bool CanInteract()
    {
        return !IsDropped;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        if (!CanInteract())
        {
            return;
        }
        KnockVending();
    }

    public void UpdateNotification(float currentInteractTime)
    {
        Debug.Log($"Interact Time = {currentInteractTime}");
        _notificationImage.fillAmount = Mathf.Clamp01(currentInteractTime / _interactTime);
    }

    private void KnockVending()
    {
        Debug.Log("Vending Knocked");
        IsDropped = true;
        _spriteRenderer.sprite = _knockedDownSprite;
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

    private void ShowHideIcon(bool showIcon)
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
}
