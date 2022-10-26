using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private ScoreManager _scoreManager;
    // enemy health
    private float _health;
    // Starting default health
    [SerializeField] float _defaultHealth = 3f;
    [Header("Player Score Decrements")]
    // Score increase for injuring enemy
    [SerializeField] int _enemyInjured = 5;
    // Score increase for killing enemy
    [SerializeField] int _enemyKilled = 20;

    private void Awake()
    {
        _scoreManager = FindObjectOfType<ScoreManager>();
        _health = _defaultHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        _health -= damageAmount;
        Debug.Log("take damage enemy");

        if (_health == 0)
        {
            _scoreManager.IncreaseScore(_enemyKilled);
            gameObject.SetActive(false);
            _health = _defaultHealth;
        }
        else
        {
            _scoreManager.IncreaseScore(_enemyInjured);
        }
    }
}
