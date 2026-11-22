using UnityEngine;

public interface IInteractable
{
    public void Interact();
    public bool CanInteract();
    public void OnFocus();
    public void OffFocus();
    //public void SendSound();
    public void UpdateNotification(float currentInteractTime);
    public float InteractTime { get; }
}
