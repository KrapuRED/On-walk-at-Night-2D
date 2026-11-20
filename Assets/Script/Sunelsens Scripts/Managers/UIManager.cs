using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ControllerPlayer _controllerPlayer;
    [SerializeField] private StaminaPlayer _staminaPlayer;
    [SerializeField] private Slider _staminaBar;

    private void OnEnable()
    {
        _controllerPlayer.OnInitialize += Initialize;
        _staminaPlayer.OnStaminaChanged += UpdateStaminaUI;
    }

    private void OnDisable()
    {
        _controllerPlayer.OnInitialize -= Initialize;
        _staminaPlayer.OnStaminaChanged -= UpdateStaminaUI;
    }

    private void Initialize(StatPlayer statPlayer)
    {
        _staminaBar.maxValue = statPlayer.MaxStamina;
        _staminaBar.value = statPlayer.MaxStamina;
    }

    private void UpdateStaminaUI(float currStamina)
    {
        _staminaBar.value = currStamina;
    }
}
