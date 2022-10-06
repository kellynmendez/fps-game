using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level01Controller : MonoBehaviour
{
    [SerializeField] Text _currentScoreTextView;
    int _currentScore;
    string _highScoreVar = "HighScore";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            IncreaseScore(5);
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ReloadLevel();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitLevel();
        }
    }

    public void IncreaseScore(int scoreIncrease)
    {
        // increase score
        _currentScore += scoreIncrease;
        // update score display so we can see the new score
        _currentScoreTextView.text = "Score: " + _currentScore.ToString();
    }

    void ReloadLevel()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
    }

    public void ExitLevel()
    {
        // compare score to high score
        int highScore = PlayerPrefs.GetInt(_highScoreVar);
        if (_currentScore > highScore)
        {
            // save current score as new high score
            PlayerPrefs.SetInt(_highScoreVar, _currentScore);
            Debug.Log("New high score: " + _currentScore);
        }

        // load new level
        SceneManager.LoadScene("MainMenu");
    }
}
