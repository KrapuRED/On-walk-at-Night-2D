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
        OnVictory?.Invoke();
    }

    public void Defeat()
    {
        OnDefeat?.Invoke();
    }
}
