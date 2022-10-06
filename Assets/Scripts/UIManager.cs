using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider _healthSlider;
    [SerializeField] GameObject _deadText;

    bool _playerDead = false;

    public void SetHealthSliderToMax(int healthMax)
    {
        _healthSlider.value = healthMax;
    }

    public void DecreaseHealth(int healthDecr)
    {
        _healthSlider.value -= healthDecr;

        if (_healthSlider.value == 0 && !_playerDead)
        {
            _playerDead = true;
            _deadText.SetActive(true);
        }
    }
}
