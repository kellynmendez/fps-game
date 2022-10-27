using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] Collider _triggerToDisable = null;
    [SerializeField] GameObject _artToDisable = null;

    [Header("Score")]
    [SerializeField] int _scoreIncr = 20;

    [Header("Feedback")]
    [SerializeField] AudioClip _collectibleSFX = null;
    [SerializeField] ParticleSystem _collectibleParticle = null;

    AudioSource _audioSource = null;
    ScoreManager _scoreManager;
    bool _active = true;

    private void Awake()
    {
        _scoreManager = FindObjectOfType<ScoreManager>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _triggerToDisable.enabled = true;
        _artToDisable.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _scoreManager.IncreaseScore(_scoreIncr);
            // Disabling everything
            _triggerToDisable.enabled = false;
            _artToDisable.SetActive(false);
            _active = false;
            PlayFX();
        }
    }

    public bool IsActive()
    {
        return _active;
    }


    public void Reactivate()
    {
        _triggerToDisable.enabled = true;
        _artToDisable.SetActive(true);
        _active = true;
    }

    void PlayFX()
    {
        // play gfx
        if (_collectibleParticle != null)
        {
            _collectibleParticle.Play();
        }
        // play sfx
        if (_audioSource != null && _collectibleSFX != null)
        {
            _audioSource.PlayOneShot(_collectibleSFX, _audioSource.volume);
        }
    }
}