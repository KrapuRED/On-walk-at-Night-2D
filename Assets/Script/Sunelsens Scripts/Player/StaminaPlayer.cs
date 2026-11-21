using System;
using UnityEngine;

public class StaminaPlayer : MonoBehaviour
{
    [SerializeField] private ControllerPlayer _controllerPlayer;

    [SerializeField] private float _maxStamina;
    [SerializeField] private float _currentStamina;
    private float _staminaRegen;
    private float _currentDrainRate;
    private float _regenWaitTime;
    private float _currentWaitTime = 0f;
    private PlayerState _currentState;

    public event Action<float> OnStaminaChanged;

    private void OnEnable()
    {
        _controllerPlayer.OnInitialize += Initialize;
        _controllerPlayer.OnStateChanged += UpdatePlayerState;
        _controllerPlayer.OnSprint += SetDrainStamina;
        
    }

    private void OnDisable()
    {
        _controllerPlayer.OnInitialize -= Initialize;
        _controllerPlayer.OnSprint -= SetDrainStamina;
        _controllerPlayer.OnStateChanged -= UpdatePlayerState;
    }

    private void Initialize(StatPlayer statPlayer)
    {
        Debug.Log("Stamina Initialize");
        _maxStamina = statPlayer.MaxStamina;
        _currentStamina = statPlayer.MaxStamina;
        _staminaRegen = statPlayer.StaminaRegen;
        _regenWaitTime = statPlayer.StaminaRegenWaitTime;
    }

    private void SetDrainStamina(float drainRate)
    {
        _currentDrainRate = drainRate;
    }

    private void DrainStamina()
    {
        _currentStamina -= _currentDrainRate * Time.deltaTime;
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);
        OnStaminaChanged?.Invoke(_currentStamina);
    }

    private void RegenStamina()
    {
        _currentStamina += _staminaRegen * Time.deltaTime;
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);
        OnStaminaChanged?.Invoke(_currentStamina);
    }

    private void UpdatePlayerState(PlayerState newPlayerState)
    {
        _currentState = newPlayerState;
    }

    private void Update()
    {
        if (_currentState == PlayerState.Sprinting)
        {
            _currentWaitTime = 0f;
            DrainStamina();
        }
        else
        {
            _currentWaitTime += Time.deltaTime;
            if (_currentWaitTime >= _regenWaitTime)
            {
                RegenStamina();
            }
        }
    }
}
