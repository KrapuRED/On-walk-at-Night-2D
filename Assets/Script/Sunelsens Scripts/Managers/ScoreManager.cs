using System;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public Dictionary<string, LevelData> LevelData = new Dictionary<string, LevelData>();
    public event Action<float> OnScoreChange;
    [SerializeField] private int _totalLevel;
    private string _currName = null;
    private float _currScore = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("Score mnager is Instanced");
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
        for (int i = 1; i <= _totalLevel; i++)
        {
            string name = $"Main_Level_{i}Score";
            if (!LevelData.ContainsKey(name))
            {
                LevelData.Add(name, new LevelData());
            }
        }

        _currName = SceneManager.GetActiveScene().name + "Score";
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
        if (_currScore > LevelData[_currName].BestScore)
        {
            LevelData[_currName].BestScore = _currScore;
            Debug.Log($"Best score added in {LevelData[_currName]} = {LevelData[_currName].BestScore}");
        }
        Debug.Log(LevelData[_currName].BestScore);
    }

    private void ChangeLevelFinishedData()
    {
        LevelData[_currName].IsFinished = true;
    }
}
