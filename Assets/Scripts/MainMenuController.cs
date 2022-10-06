using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] AudioClip _startingSong;
    [SerializeField] Text _highScoreTextView;
    string _highScoreVar = "HighScore";

    // Start is called before the first frame update
    void Start()
    {
        // load high score display
        int highScore = PlayerPrefs.GetInt(_highScoreVar);
        _highScoreTextView.text = highScore.ToString();

        if(_startingSong != null)
        {
            AudioManager.Instance.PlaySong(_startingSong);
        }
    }

    public void ResetData()
    {
        PlayerPrefs.SetInt(_highScoreVar, 0);
        int highScore = PlayerPrefs.GetInt(_highScoreVar);
        _highScoreTextView.text = highScore.ToString();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
