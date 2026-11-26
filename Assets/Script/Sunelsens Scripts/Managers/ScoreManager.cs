using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [Header("Read Only")]
    [SerializeField] private float _score = 0f;
    public event Action<float> OnScoreChange;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    public void AddScore(float score)
    {
        _score += score;
        OnScoreChange?.Invoke(_score);
    }
}
