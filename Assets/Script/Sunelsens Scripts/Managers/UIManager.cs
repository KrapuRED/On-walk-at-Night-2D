using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ControllerPlayer _controllerPlayer;
    [SerializeField] private StaminaPlayer _staminaPlayer;
    [SerializeField] private Slider _staminaBar;
    [SerializeField] private CanvasGroup _victoryCanvasGroup;
    [SerializeField] private CanvasGroup _defeatCanvasGroup;

    private void OnEnable()
    {
        _controllerPlayer.OnInitialize += Initialize;
        _staminaPlayer.OnStaminaChanged += UpdateStaminaUI;
        GameManager.Instance.OnVictory += ShowVictoryUI;
        GameManager.Instance.OnDefeat += ShowDefeatUI;
    }

    private void OnDisable()
    {
        _controllerPlayer.OnInitialize -= Initialize;
        _staminaPlayer.OnStaminaChanged -= UpdateStaminaUI;
        GameManager.Instance.OnVictory -= ShowVictoryUI;
        GameManager.Instance.OnDefeat -= ShowDefeatUI;
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

    private void ShowVictoryUI()
    {
        OpenPanel(_victoryCanvasGroup);
    }

    private void ShowDefeatUI()
    {
        OpenPanel(_defeatCanvasGroup);
    }

    private void ClosePanel(CanvasGroup panel)
    {
        panel.alpha = 0f;
        panel.interactable = false;
        panel.blocksRaycasts = false;
    }

    private void OpenPanel(CanvasGroup panel)
    {
        panel.alpha = 1f;
        panel.interactable = true;
        panel.blocksRaycasts = true;
    }
}
