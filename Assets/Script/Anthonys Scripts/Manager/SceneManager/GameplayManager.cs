using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayBgm(AudioManager.instance.Gameplay_BGM);
        }
        else
        {
            Debug.Log("Belum ada AUDIO MANAGER");
        }
    }
}
