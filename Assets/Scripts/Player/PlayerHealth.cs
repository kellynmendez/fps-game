using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private UIManager _uiManager;
    bool _isInvincible = false;
    [SerializeField] int _healthMax = 200;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
        SetHealthToMax();
    }

    public void SetIsInvincible(bool invincibility)
    {
        _isInvincible = invincibility;
    }

    private void SetHealthToMax()
    {
        _uiManager.SetHealthSliderToMax(_healthMax);
    }

    public void DamagePlayer(int damageAmount)
    {
        if (!_isInvincible)
        {
            _uiManager.DecreaseHealth(damageAmount);
        }
    }
}
