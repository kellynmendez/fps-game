using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private UIManager _uiManager;
    int _healthMax = 100;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
        SetHealthToMax();
    }

    private void SetHealthToMax()
    {
        _uiManager.SetHealthSliderToMax(_healthMax);
    }

    public void DamagePlayer(int damageAmount)
    {
        _uiManager.DecreaseHealth(damageAmount);
    }
}
