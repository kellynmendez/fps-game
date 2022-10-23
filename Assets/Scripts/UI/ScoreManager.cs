using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text _currentScoreTextView;
    private UIManager _uiManager;
    int _currentScore;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    public void IncreaseScore(int scoreIncrease)
    {
        // increase score
        _currentScore += scoreIncrease;
        // update score display so we can see the new score
        _currentScoreTextView.text = "Score: " + _currentScore.ToString();
    }

    public int GetCurrentScore()
    {
        return _currentScore;
    }
}
