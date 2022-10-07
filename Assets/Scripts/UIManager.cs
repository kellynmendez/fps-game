using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider _healthSlider;
    [SerializeField] GameObject _pausePanel;
    [SerializeField] GameObject _deadPromptPanel;

    bool _playerDead;

    private void Awake()
    {
        _playerDead = false;
    }

    public void SetHealthSliderToMax(int healthMax)
    {
        _healthSlider.value = healthMax;
    }

    public void DecreaseHealth(int healthDecr)
    {
        _healthSlider.value -= healthDecr;
        Debug.Log("player health is " + _healthSlider.value);
        if (_healthSlider.value == 0 && !_playerDead)
        {
            _playerDead = true;
            Cursor.lockState = CursorLockMode.None;
            _deadPromptPanel.SetActive(true);
        }
    }

    public void PauseGame()
    {
        _pausePanel.SetActive(true);
    }

    public void PlayGame()
    {
        _pausePanel.SetActive(false);
    }

    public bool IsPlayerDead()
    {
        return _playerDead;
    }
}
