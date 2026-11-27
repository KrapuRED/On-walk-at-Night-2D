using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if(AudioManager.instance != null)
        {
            AudioManager.instance.PlayBgm(AudioManager.instance.MainMenu_BGM);
        }
        else
        {
            Debug.Log("Belum ada AUDIO MANAGER");
        }
    }
}
