using System.Collections;
using UnityEngine;

public class MetalTrashBin : InteractableObject
{
    [SerializeField] private AnimatorTrashBin _animatorTrashBin;

    public override void Interact()
    {
        base.Interact();
        KickBin();
    }

    private void KickBin()
    {
        Debug.Log("Bin Kicked");
        IsInteracted = true;
        _animatorTrashBin.AnimationTrashBin_On();
        AudioManager.instance.PlaySFX(AudioManager.instance.TrashBinKicked_SFX);
    }

    public void GoBack()
    {
        IsInteracted = false;
    }
}
