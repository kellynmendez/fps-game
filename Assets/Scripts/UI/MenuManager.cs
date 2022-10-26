using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private UIManager _uiManager;
    private ScoreManager _scoreManager;
    string _highScoreVar = "HighScore";

    bool _pause = false;
    bool _playerIsDead = false;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _scoreManager = FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
        // Checking if player is dead
        _playerIsDead = _uiManager.IsPlayerDead();
        // If the player isn't dead and wants to pause
        if (!_playerIsDead && Input.GetKeyDown(KeyCode.Escape))
        {
            // Pause game, show menu, and unlock the cursor
            _pause = true;
            Cursor.lockState = CursorLockMode.None;
            _uiManager.PauseGame();
            // Freeze the screen
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        // Resume the game
        _uiManager.PlayGame();
        // Unfreeze the screen
        Time.timeScale = 1;

        // If paused, then unpause and lock cursor
        if (_pause)
        {
            _pause = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ReloadLevel()
    {
        // If you died and are restarting the level, save the high score if needed
        if (!_pause)
        {
            SaveHighScoreIfEligible();
        }
        // If paused, then unpause and lock cursor
        else
        {
            _pause = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Reloading the level
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
        // Unfreezing screen
        Time.timeScale = 1;
    }

    public void ExitLevel()
    {
        // New high score if current score is higher than high score
        //   and exiting after finishing game (not from pause)
        if (!_pause)
        {
            SaveHighScoreIfEligible();
        }
        // Lock cursor
        Cursor.lockState = CursorLockMode.None;
        // Unfreeze the screen
        Time.timeScale = 1;
        // Unpause if paused
        if (_pause)
        {
            _pause = false;
        }

        // Load the main menu
        SceneManager.LoadScene("MainMenu");
    }

    private void SaveHighScoreIfEligible()
    {
        int highScore = PlayerPrefs.GetInt(_highScoreVar);
        int currScore = _scoreManager.GetCurrentScore();
        // Check if high score is less than the current score
        if (currScore > highScore)
        {
            // save current score as new high score
            PlayerPrefs.SetInt(_highScoreVar, currScore);
            Debug.Log("New high score: " + currScore);
        }
    }
}
