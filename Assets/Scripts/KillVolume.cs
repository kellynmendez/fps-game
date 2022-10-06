using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : MonoBehaviour
{
    [SerializeField] string loseText = "You lost!";

    [Header("Setup")]
    [SerializeField] GameObject _visualsToDeactivate = null;

    Collider _colliderToDeactivate = null;

    private void Awake()
    {
        // get collider
        _colliderToDeactivate = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //PlayerShip playerShip = other.gameObject.GetComponent<PlayerShip>();
            //bool _mushroomMode = playerShip.InMushroomMode();
                // Disabling debris
                DisableObject();
                // adding to score
                //playerShip.UpdateScore(_mushroomScoreIncr);
            // If we found something valid, continue
                //_uiController.ShowText(loseText);
                //playerShip.Kill(false);
        }
    }

    public void DisableObject()
    {
        // disable collider so it can't be retriggered
        _colliderToDeactivate.enabled = false;
        // disable visuals to simulate deactivated
        _visualsToDeactivate.SetActive(false);
        // deactivate particle flash/audio
        PlayFX();
    }

    private void PlayFX()
    {
        // play gfx
        //if (_explodeParticle != null)
        //{
        //    _explodeParticle.Play();
        //}
        // play sfx
        //if (_audioSource != null && _mushroomSFX != null)
        //{
        //   _audioSource.PlayOneShot(_mushroomSFX, _audioSource.volume);
        //}
    }
}
