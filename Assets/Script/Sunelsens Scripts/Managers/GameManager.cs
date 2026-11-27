using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event Action OnVictory;
    public event Action OnDefeat;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Victory()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.LevelComplete_SFX);
        }
        OnVictory?.Invoke();
    }

    public void Defeat()
    {
        if(AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.PlayerDead_SFX);
        }
        OnDefeat?.Invoke();
    }
}
