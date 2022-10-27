using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    private ScoreManager _scoreManager;
    // enemy health
    private float _health;
    // Enemy navmesh
    private NavMeshAgent _agent;
    // Starting default health
    [SerializeField] float _defaultHealth = 3f;

    [Header("Player Score Decrements")]
    // Score increase for injuring enemy
    [SerializeField] int _enemyInjured = 5;
    // Score increase for killing enemy
    [SerializeField] int _enemyKilled = 20;

    [Header("Knockback")]
    // Knockback force on damage
    [SerializeField] float _knockbackForce = 4f;
    // Knockback speed on damage
    [SerializeField] float _knockbackSpeed = 40f;
    // Knockback acceleration on damage
    [SerializeField] float _knockbackAcceleration = 20f;
    // Knockback time
    [SerializeField] float _knockbackTime = 0.3f;

    [Header("Setup")]
    [SerializeField] GameObject _visualsToDeactivate = null;

    [Header("Feedback")]
    [SerializeField] AudioClip _hurtFX = null;
    [SerializeField] AudioClip _deadFX = null;
    [SerializeField] ParticleSystem _deadParticle = null;
    AudioSource _audioSource = null;

    // Knockback boolean
    private bool _knockback;
    // Knockback direction
    private Vector3 _kbDirection;
    // Knockback default values
    private float _defaultSpeed;
    private float _defaultAngularSpeed;
    private float _defaultAcceleration;


    private void Awake()
    {
        _scoreManager = FindObjectOfType<ScoreManager>();
        _agent = GetComponent<NavMeshAgent>();
        _health = _defaultHealth;
        _knockback = false;
    }

    private void Start()
    {
        _defaultSpeed = _agent.speed;
        _defaultAngularSpeed = _agent.angularSpeed;
        _defaultAcceleration = _agent.acceleration;
    }

    private void FixedUpdate()
    {
        if (_knockback)
        {
            _agent.velocity = _kbDirection * _knockbackForce;
        }
    }

    public bool WasEnemyHit()
    {
        return _knockback;
    }

    public void TakeDamage(float damageAmount)
    {
        _health -= damageAmount;
        Debug.Log("take damage enemy");

        if (_health == 0)
        {
            _knockback = true;
            StartCoroutine(WaitForParticlesToDie());
            _visualsToDeactivate.SetActive(false);
            PlayDeadFX();
            _scoreManager.IncreaseScore(_enemyKilled);
            _health = _defaultHealth;
        }
        else
        {
            _scoreManager.IncreaseScore(_enemyInjured);
            PlayDamageFX();
        }
    }

    public void EnemyKnockback(Vector3 direction)
    {
        _kbDirection = direction;
        if (gameObject.activeSelf)
        {
            StartCoroutine(Knockback());
        }
    }

    IEnumerator Knockback()
    {
        _knockback = true;
        _agent.angularSpeed = 0; // keep from spinning
        _agent.speed = _knockbackSpeed;
        _agent.acceleration = _knockbackAcceleration;

        yield return new WaitForSeconds(_knockbackTime);

        // resetting back to defult values
        _knockback = false;
        _agent.angularSpeed = _defaultAngularSpeed;
        _agent.velocity = _kbDirection * 0;
        _agent.speed = _defaultSpeed;
        _agent.acceleration = _defaultAcceleration;
    }

    IEnumerator WaitForParticlesToDie()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        _visualsToDeactivate.SetActive(true);
    }

    public void PlayDamageFX()
    {
        // play sfx
        if (_audioSource != null && _hurtFX != null)
        {
            _audioSource.PlayOneShot(_deadFX, _audioSource.volume);
        }
    }

    public void PlayDeadFX()
    {
        Debug.Log("playing enemy dead fx");
        // play gfx
        if (_deadParticle != null)
        {
            _deadParticle.Play();
        }
        // play sfx
        if (_audioSource != null && _deadFX != null)
        {
            _audioSource.PlayOneShot(_deadFX, _audioSource.volume);
        }
    }
}
