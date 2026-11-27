using System.Collections;
using UnityEngine;

public class MetalTrashBin : InteractableObject
{
    [SerializeField] private Sprite _kickedSprite;
    [SerializeField] private Sprite _upSprite;
    [SerializeField] private AnimatorTrashBin _animatorTrashBin;
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
        StartCoroutine(WaitToGoBack());
        AudioManager.instance.PlaySFX(AudioManager.instance.TrashBinKicked_SFX);
    }
    private void Update()
    {
        if (_isInteracting)
        {
            _animatorTrashBin.AnimationTrashBin_On();
        }
        else
        {
            _animatorTrashBin.AnimationTrashBin_Off();
        }
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
