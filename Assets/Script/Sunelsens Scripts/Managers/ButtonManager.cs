using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private CanvasGroup _mainMenuPanel;
    [SerializeField] private CanvasGroup _settingPanel;
    [SerializeField] private CanvasGroup _creditPanel;

    [Header("Pause")]
    [SerializeField] private CanvasGroup _pausePanel;
    [SerializeField] private CanvasGroup _settingPausePanel;

    [Header("Misc")]
    [SerializeField] private InputPlayer _inputPlayer;

    private void OnEnable()
    {
        if (_inputPlayer != null)
        {
            _inputPlayer.OnPauseInput += OpenClosePause;
        }
        
    }

    private void OnDisable()
    {
        if (_inputPlayer != null)
        {
            _inputPlayer.OnPauseInput -= OpenClosePause;
        }
    }

    public void PlayGame()
    {
        TransitionManager.Instance.LoadScene("SceneSunelsen");
    }

    public void ExitGame()
    {
        //PlayButtonSFX();
        Application.Quit();
    }

    public void OpenCloseMainMenuSettings()
    {
        //PlayButtonSFX();
        if (_settingPanel.alpha == 1f)
        {
            ClosePanel(_settingPanel);
            OpenPanel(_mainMenuPanel);
        }
        else
        {
            ClosePanel(_mainMenuPanel);
            OpenPanel(_settingPanel);
        }
    }

    public void OpenClosePause()
    {
        //PlayButtonSFX();
        if (_pausePanel.alpha == 1f)
        {
            Time.timeScale = 1f;
            ClosePanel(_settingPausePanel);
            ClosePanel(_pausePanel);
        }
        else
        {
            Time.timeScale = 0f;
            OpenPanel(_pausePanel);
        }
    }

    public void OpenClosePauseSetting()
    {
        //PlayButtonSFX();
        if (_settingPausePanel.alpha == 1f)
        {
            ClosePanel(_settingPausePanel);
            OpenPanel(_pausePanel);
        }
        else
        {
            OpenPanel(_settingPausePanel);
            ClosePanel(_pausePanel);
        }
    }

    public void OpenCloseCredit()
    {
        //PlayButtonSFX();
        if (_creditPanel.alpha == 1f)
        {
            ClosePanel(_creditPanel);
            OpenPanel(_mainMenuPanel);
        }
        else
        {
            ClosePanel(_mainMenuPanel);
            OpenPanel(_creditPanel);
        }
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
