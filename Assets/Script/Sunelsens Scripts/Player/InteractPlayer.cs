using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPlayer : MonoBehaviour
{
    [SerializeField] private InputPlayer _inputPlayer;
    //[SerializeField] private MovementPlayer _movementPlayer;
    [SerializeField] private ControllerPlayer _controllerPlayer;
    private List<Collider2D> _nearColliders = new();
    private IInteractable _currentInteractable = null;
    private float _currentInteractTimer = 0f;
    private bool _isInteracting = false;

    private void OnEnable()
    {
        _inputPlayer.OnInteractInput += UpdateInteractState;
        //_movementPlayer.OnMove += CalculateClosest;
    }

    private void OnDisable()
    {
        _inputPlayer.OnInteractInput -= UpdateInteractState;
        //_movementPlayer.OnMove -= CalculateClosest;
    }

    private void Update()
    {
        CalculateClosest();
        if (_isInteracting && _currentInteractable != null)
        {
            _currentInteractTimer += Time.deltaTime;
            _currentInteractable?.UpdateNotification(_currentInteractTimer);
            if (_currentInteractTimer >= _currentInteractable?.InteractTime)
            {
                _currentInteractable.Interact();
                _currentInteractable.OffFocus();
                UpdateInteractState(false);
                _currentInteractable = null;
                
            }
        }  
    }

    private void UpdateInteractState(bool isInteracting)
    {
        _isInteracting = isInteracting;
        _currentInteractTimer = 0f;
        _currentInteractable?.UpdateNotification(_currentInteractTimer);
        if (_isInteracting && _currentInteractable != null)
        {
            _controllerPlayer.ChangeState(PlayerState.Interacting);
        }
        else
        {
            _controllerPlayer.ChangeState(PlayerState.Idle);
        }
    }

    private void CalculateClosest()
    {
        if (_currentInteractable != null && _controllerPlayer.GetCurrentState() == PlayerState.Interacting)
        {
            return;
        }
        IInteractable closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D collider in _nearColliders)
        {
            float distance = Vector2.Distance(transform.position, collider.ClosestPoint(transform.position));

            if (distance < closestDistance && collider.GetComponent<IInteractable>().CanInteract())
            {
                closestDistance = distance;
                collider.TryGetComponent<IInteractable>(out closestInteractable);
            }
        }

        if (_currentInteractable == closestInteractable)
        {
            return;
        }

        _currentInteractable?.OffFocus();
        _currentInteractable = closestInteractable;
        _currentInteractable?.OnFocus();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out IInteractable interactable) && interactable.CanInteract())
        {
            _nearColliders.Add(collision);
            Debug.Log(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            _nearColliders.Remove(collision);
        }
    }
}
