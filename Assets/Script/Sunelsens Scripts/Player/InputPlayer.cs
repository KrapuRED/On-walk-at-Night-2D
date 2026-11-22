using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputPlayer : MonoBehaviour
{
    public event Action<Vector2> OnMovementInput;
    public event Action OnPauseInput;
    public event Action<bool> OnSprintInput;
    public event Action<bool> OnInteractInput;
    public void Movement(InputAction.CallbackContext ctx)
    {
        Debug.Log("Move Inputted");
        OnMovementInput?.Invoke(ctx.ReadValue<Vector2>());
    }

    public void Pause(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("Paused");
            OnPauseInput?.Invoke();
        }
    }

    public void Sprint(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            OnSprintInput?.Invoke(true);
        }
        else if (ctx.canceled)
        {
            OnSprintInput?.Invoke(false);
        }
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("Is Interacting");
            OnInteractInput?.Invoke(true);
        }
        else if (ctx.canceled)
        {
            OnInteractInput?.Invoke(false);
        }
    }
}
