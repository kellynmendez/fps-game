using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level01Controller : MonoBehaviour
{
    [SerializeField] Text _currentScoreTextView;
    [SerializeField] UIManager _uiManager;
    int _currentScore;
    int _scoreIncr = 5;
    string _highScoreVar = "HighScore";

    bool _pause = false;
    bool _playerIsDead = false;

    void Update()
    {
        // Checking if player is dead
        _playerIsDead = _uiManager.IsPlayerDead();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            IncreaseScore(_scoreIncr);
        }
        if (!_playerIsDead && Input.GetKeyDown(KeyCode.Escape))
        {
            _pause = true;
            Cursor.lockState = CursorLockMode.None;
            _uiManager.PauseGame();
            Time.timeScale = 0;
        }
    }

    public void IncreaseScore(int scoreIncrease)
    {
        // increase score
        _currentScore += scoreIncrease;
        // update score display so we can see the new score
        _currentScoreTextView.text = "Score: " + _currentScore.ToString();
    }

    public void ResumeGame()
    {
        _uiManager.PlayGame();
        Time.timeScale = 1;

        if (_pause)
        {
            _pause = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ReloadLevel()
    {
        if (!_pause)
        {
            // if you died and are restarting the level, save score
            int highScore = PlayerPrefs.GetInt(_highScoreVar);
            if (_currentScore > highScore)
            {
                // save current score as new high score
                PlayerPrefs.SetInt(_highScoreVar, _currentScore);
                Debug.Log("New high score: " + _currentScore);
            }
        }

        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
        Time.timeScale = 1;

        if (_pause)
        {
            _pause = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
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
        Cursor.lockState = CursorLockMode.None;

        // if from pause
        Time.timeScale = 1;
        if (_pause)
        {
            _pause = false;
        }

        // load new level
        SceneManager.LoadScene("MainMenu");
    }
}
