using System;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Walking,
    Sprinting,
    Interacting,
    Dead
}

public class StatePlayer : MonoBehaviour
{
    [SerializeField] protected PlayerState _currentState;
    public void ChangeState(PlayerState state)
    {
        if (_currentState == state)
        {
            return;
        }
        _currentState = state;
        CheckState();
    }

    private void CheckState()
    {
        switch (_currentState)
        {
            case PlayerState.Idle:
                PlayerIdle();
                break;

            case PlayerState.Walking:
                PlayerWalking();
                break;

            case PlayerState.Sprinting:
                PlayerSprinting();
                break;

            case PlayerState.Interacting:
                PlayerInteracting();
                break;

            case PlayerState.Dead:
                PlayerDead();
                break;
        }
    }

    protected virtual void PlayerIdle() 
    {
        Debug.Log("Player Idle");
    }
    protected virtual void PlayerWalking()
    {
        Debug.Log("Player Walk");
    }
    protected virtual void PlayerSprinting()
    {
        Debug.Log("Player Sprint");
    }
    protected virtual void PlayerInteracting()
    {
        Debug.Log("Player Interact");
    }
    protected virtual void PlayerDead()
    {
        Debug.Log("Player Dead");
    }
}
