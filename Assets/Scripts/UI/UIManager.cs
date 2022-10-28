using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider _healthSlider;
    [SerializeField] GameObject _pausePanel;
    [SerializeField] GameObject _deadPromptPanel;
    [Header("Damage Graphic")]
    [SerializeField] MaskableGraphic _graphic = null;
    [SerializeField] float _fadeInTime = 0.2f;
    [SerializeField] float _fadeOutTime = 0.2f;
    [Header("Feedback")]
    [SerializeField] AudioClip _deadFX = null;
    [SerializeField] AudioClip _hurtFX = null;
    AudioSource _audioSource = null;

    bool _playerDead;
    Color _oldColor;
    float _startAlpha;
    float _endAlpha = 0.2f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _playerDead = false;
        _oldColor = _graphic.color;
        _startAlpha = _graphic.color.a;
    }

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
            Cursor.lockState = CursorLockMode.None;
            _deadPromptPanel.SetActive(true);
            PlayDeadFX();
        }
        else
        {
            StartCoroutine(PlayDamageFX());
        }
    }

    IEnumerator PlayDamageFX()
    {
        // play sfx
        if (_audioSource != null && _hurtFX != null)
        {
            _audioSource.PlayOneShot(_hurtFX, _audioSource.volume);
        }

        Color deadGraphic = new Color(1, 0, 0, .2f);

        // lerp alpha value to show up
        float elapsedTime = 0;
        while (elapsedTime < _fadeInTime)
        {
            deadGraphic.a = Mathf.Lerp(_startAlpha, _endAlpha, elapsedTime / _fadeInTime);
            _graphic.color = deadGraphic;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        deadGraphic.a = _endAlpha;
        _graphic.color = deadGraphic;

        // lerp alpha value to disappear
        elapsedTime = 0;
        while (elapsedTime < _fadeOutTime)
        {
            deadGraphic.a = Mathf.Lerp(_endAlpha, _startAlpha, elapsedTime / _fadeOutTime);
            _graphic.color = deadGraphic;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        _graphic.color = _oldColor;
    }

    public void PlayDeadFX()
    {
        Debug.Log("play dead sound");
        // play sfx
        if (_audioSource != null && _deadFX != null)
        {
            _audioSource.PlayOneShot(_deadFX, _audioSource.volume);
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

    public MaskableGraphic GetMaskableGraphic()
    {
        return _graphic;
    }
}
