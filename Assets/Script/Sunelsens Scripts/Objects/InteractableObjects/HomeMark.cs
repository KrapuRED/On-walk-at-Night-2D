using UnityEngine;

public class HomeMark : InteractableObject
{
    public override void Interact()
    {
        if (!CanInteract())
        {
            return;
        }
        GoHome();
    }
    private void Update()
    {
        if (_isInteracting)
        {
            Debug.Log("Make Sound");
            SendSound();
        }
    }



    private void GoHome()
    {
        Debug.Log("Went Home");
        IsInteracted = true;
        GameManager.Instance.Victory();
    }
}
