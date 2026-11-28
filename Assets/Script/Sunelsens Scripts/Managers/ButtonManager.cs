using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private CanvasGroup _mainMenuPanel;
    [SerializeField] private CanvasGroup _settingPanel;

    [Header("Pause")]
    [SerializeField] private CanvasGroup _pausePanel;
    [SerializeField] private CanvasGroup _settingPausePanel;

    [Header("Misc")]
    [SerializeField] private InputPlayer _inputPlayer;

    [Header("Level Name")]
    [SerializeField] private string Level1Name;
    [SerializeField] private string Level2Name;
    [SerializeField] private string Level3Name;

    private void OnEnable()
    {
        if (_inputPlayer != null)
        {
            _inputPlayer.OnPauseInput += OpenClosePause;
        }
        else
        {
            Debug.Log("Input player is null");
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
        PlayButtonSFX();
        TransitionManager.Instance.LoadScene("LevelSelectScene");
    }

    public void LoadLevel1()
    {
        PlayButtonSFX();
        TransitionManager.Instance.LoadScene(Level1Name);
    }

    public void LoadLevel2()
    {
        PlayButtonSFX();
        TransitionManager.Instance.LoadScene(Level2Name);
    }

    public void LoadLevel3()
    {
        PlayButtonSFX();
        TransitionManager.Instance.LoadScene(Level3Name);
    }

    public void ExitGame()
    {
        PlayButtonSFX();
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        PlayButtonSFX();
        Time.timeScale = 1f;
        TransitionManager.Instance.LoadScene("MainMenu");
    }

    public void BackToLevelSelect()
    {
        PlayButtonSFX();
        Time.timeScale = 1f;
        TransitionManager.Instance.LoadScene("LevelSelectScene");
    }

    public void Retry()
    {
        PlayButtonSFX();
        Debug.Log("Retry Scene");
        TransitionManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenCloseMainMenuSettings()
    {
        PlayButtonSFX();
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
        PlayButtonSFX();
        Debug.Log("vhat");
        if (_pausePanel.alpha == 1f || _settingPausePanel.alpha == 1f)
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
        PlayButtonSFX();
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
        PlayButtonSFX();
        TransitionManager.Instance.LoadScene("CreditScene");
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

    private void PlayButtonSFX()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.UIClick_SFX);
        }
    }
}
