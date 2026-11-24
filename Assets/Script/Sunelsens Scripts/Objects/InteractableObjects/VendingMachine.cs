using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : InteractableObject
{
    [SerializeField] private Sprite _knockedDownSprite;

    public override void Interact()
    {
        base.Interact();
        KnockVending();
    }

    private void KnockVending()
    {
        Debug.Log("Vending Knocked");
        IsInteracted = true;
        _spriteRenderer.sprite = _knockedDownSprite;
    } 
}
