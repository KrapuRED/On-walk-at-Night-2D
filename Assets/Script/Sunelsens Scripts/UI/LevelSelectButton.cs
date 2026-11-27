using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    private Image _buttonImage;
    private Button _button;
    [SerializeField] private TMP_Text _bestScoreText;
    [SerializeField] private int _levelNumber;
    [SerializeField] private Sprite _lockedSprite;
    [SerializeField] private Sprite _unlockedSprite;

    private void Awake()
    {
        _buttonImage = GetComponent<Image>();
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        if (ScoreManager.Instance == null)
        {
            Debug.Log("ScoreManager Gone");
            return;
        }

        ScoreManager _scoreManager = ScoreManager.Instance;
        if (_levelNumber > 1)
        {
            if (!_scoreManager.LevelData[$"Main_Level_{_levelNumber - 1}Score"].IsFinished)
            {
                _buttonImage.sprite = _lockedSprite;
                _button.interactable = false;
                return;
            }
        }

        if (!_scoreManager.LevelData.ContainsKey($"Main_Level_{_levelNumber}Score"))
        {
            Debug.Log($"Main_Level_{_levelNumber}Score is Gone");
            return;
        }

        _bestScoreText.text = string.Format("{0:00000}", _scoreManager.LevelData[$"Main_Level_{_levelNumber}Score"].BestScore);
        Debug.Log($"Main_Level_{_levelNumber}Score = {_scoreManager.LevelData[$"Main_Level_{_levelNumber}Score"].BestScore}");
        _buttonImage.sprite = _unlockedSprite;
        _button.interactable = true;
    }
}
