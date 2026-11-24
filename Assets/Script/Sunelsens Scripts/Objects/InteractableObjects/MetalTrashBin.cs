using System.Collections;
using UnityEngine;

public class MetalTrashBin : InteractableObject
{
    [SerializeField] private Sprite _kickedSprite;
    [SerializeField] private Sprite _upSprite;
    //[SerializeField] private Animator _animator;
    //[SerializeField] private float _waitSeconds;

    public override void Interact()
    {
        base.Interact();
        KickBin();
    }

    private void KickBin()
    {
        Debug.Log("Bin Kicked");
        IsInteracted = true;
        _spriteRenderer.sprite = _kickedSprite;//animation start
        //_animator.SetTrigger();
        StartCoroutine(WaitToGoBack());
    }

    IEnumerator WaitToGoBack()
    {
        yield return new WaitForSeconds(1f);
        GoBack();
    }

    public void GoBack()
    {
        _spriteRenderer.sprite = _upSprite;
        IsInteracted = false;
    }
}
