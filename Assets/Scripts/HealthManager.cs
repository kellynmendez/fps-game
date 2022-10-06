using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] GameObject _visualsToDeactivate;
    [SerializeField] ParticleSystemForceField _field;
    [SerializeField] int _currentHealth = 250;


    private bool _playerIsDead;

    private void Awake()
    {
        _playerIsDead = false;
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void DecreaseHealth(int healthDecr)
    {
        _currentHealth -= healthDecr;
        if (_currentHealth <= 0)
        {
            Kill();
        }
    }

    public bool IsPlayerDead()
    {
        return _playerIsDead;
    }

    private void Kill()
    {
        DisableDeathObjects();
        _playerIsDead = true;
    }

    private void DisableDeathObjects()
    {
        _visualsToDeactivate.SetActive(false);
    }
}
