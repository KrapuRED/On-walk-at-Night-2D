using System;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private Dictionary<string, LevelData> _levelData = new Dictionary<string, LevelData>();
    public event Action<float> OnScoreChange;
    [SerializeField] private int _totalLevel;
    private string _currName = null;
    private float _currScore = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        for (int i = 1; i < _totalLevel; i++)
        {
            string name = $"Main_level_{i}Score";
            if (!_levelData.ContainsKey(name))
            {
                _levelData.Add(name, new LevelData());
            }
        }
    }

    private void OnEnable()
    {
        TransitionManager.Instance.OnSceneChanged += ChangeCurrentScoreData;
        GameManager.Instance.OnVictory += ChangeLevelFinishedData;
    }

    private void OnDisable()
    {
        TransitionManager.Instance.OnSceneChanged -= ChangeCurrentScoreData;
        GameManager.Instance.OnVictory -= ChangeLevelFinishedData;
    }

    private void ChangeCurrentScoreData(string currScene)
    {
        _currName = currScene + "Score";
        _currScore = 0f;
    }

    public void AddScore(float score)
    {
        _currScore += score;
        OnScoreChange?.Invoke(_currScore);
        if (_currScore > _levelData[_currName]._bestScore)
        {
            _levelData[_currName]._bestScore = _currScore;
        }
    }

    private void ChangeLevelFinishedData()
    {
        _levelData[_currName]._isFinished = true;
    }
}
