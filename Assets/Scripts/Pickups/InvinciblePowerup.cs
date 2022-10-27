using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvinciblePowerup : MonoBehaviour
{
    [SerializeField] Collider _triggerToDisable = null;
    [SerializeField] GameObject _artToDisable = null;
    [SerializeField] float _invincibleTime = 6f;

    [Header("Score")]
    [SerializeField] int _scoreIncr = 20;

    [Header("Feedback")]
    [SerializeField] AudioClip _collectibleSFX = null;
    [SerializeField] ParticleSystem _collectibleParticle = null;
    
    [Header("Invincible Graphic")]
    [SerializeField] float _fadeInTime = 0.2f;
    [SerializeField] float _fadeOutTime = 0.2f;
    private MaskableGraphic _graphic = null;

    AudioSource _audioSource = null;
    bool _active = true;
    Color _oldColor;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _graphic = FindObjectOfType<UIManager>().GetMaskableGraphic();
        _oldColor = _graphic.color;
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
            // Make player invincible
            StartCoroutine(InvincibleSequence(other.gameObject.GetComponent<PlayerHealth>()));
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

    IEnumerator InvincibleSequence(PlayerHealth player)
    {
        player.SetIsInvincible(true);
        Color invColor = new Color(0.5f, 0f, 0.8f, 0.2f);
        LerpColor(_graphic, _oldColor, invColor, 0.1f);
        yield return new WaitForSeconds(_invincibleTime);
        LerpColor(_graphic, invColor, _oldColor, 0.3f);
        yield return new WaitForSeconds(0.3f);
        LerpColor(_graphic, _oldColor, invColor, 0.3f);
        yield return new WaitForSeconds(0.3f);
        LerpColor(_graphic, invColor, _oldColor, 0.3f);
        yield return new WaitForSeconds(0.3f);
        LerpColor(_graphic, _oldColor, invColor, 0.3f);
        yield return new WaitForSeconds(0.3f);
        LerpColor(_graphic, invColor, _oldColor, 0.3f);
        yield return new WaitForSeconds(0.3f);
        LerpColor(_graphic, _oldColor, invColor, 0.3f);
        yield return new WaitForSeconds(0.3f);
        LerpColor(_graphic, invColor, _oldColor, 0.3f);
        yield return new WaitForSeconds(0.3f);
        LerpColor(_graphic, _oldColor, invColor, 0.3f);
        yield return new WaitForSeconds(0.3f);
        LerpColor(_graphic, invColor, _oldColor, 0.3f);
        yield return new WaitForSeconds(0.3f);
        LerpColor(_graphic, _oldColor, invColor, 0.3f);
        yield return new WaitForSeconds(0.3f);
        LerpColor(_graphic, invColor, _oldColor, 0.3f);
        player.SetIsInvincible(false);
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

    public void LerpColor(MaskableGraphic graphic, Color from, Color to, float duration)
    {
        // initial value
        graphic.color = from;

        // animate value
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            graphic.color = Color.Lerp(from, to, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
        }

        // final value
        graphic.color = to;
    }
}