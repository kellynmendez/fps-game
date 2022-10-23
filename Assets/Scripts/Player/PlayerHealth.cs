using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private UIManager _uiManager;
    int _healthEKeyDecr = 5;
    int _healthMax = 100;
    bool _playerIsDead = false;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
        SetHealthToMax();
    }

    private void Update()
    {
        if (!_playerIsDead && Input.GetKeyDown(KeyCode.E))
        {
            DamagePlayer(_healthEKeyDecr);
        }
    }

    private void SetHealthToMax()
    {
        _uiManager.SetHealthSliderToMax(_healthMax);
    }

    public void DamagePlayer(int damageAmount)
    {
        _uiManager.DecreaseHealth(damageAmount);
        _playerIsDead = _uiManager.IsPlayerDead();
    }
}
