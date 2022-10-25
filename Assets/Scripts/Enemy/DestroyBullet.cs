using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour
{

    // How long before bullet disappears
    private float _lifeTime = 3f;
    // Scale amount
    private float _scaleAmount = 0.98f;
    // Starting scale
    Vector3 _startScale;
    // Player health object
    PlayerHealth _playerHealth;
    // Health decrease for this bullet
    [SerializeField] int _damageAmount = 5;

    private void Awake()
    {
        _startScale = gameObject.transform.localScale;
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void OnEnable()
    {
        StartCoroutine(FadeToInactive());
    }

    private IEnumerator FadeToInactive()
    {
        float start = Time.time;

        while (start + _lifeTime > Time.time)
        {
            // Slowly shrink object
            gameObject.transform.localScale = gameObject.transform.localScale * _scaleAmount;
            yield return new WaitForSeconds(0.1f); // scale consistently across any machine
        }
        Debug.Log("set bullet inactive 1");
        gameObject.transform.localScale = _startScale;
        gameObject.SetActive(false);
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "EnemyBody")
        {
            Debug.Log("set bullet inactive + " + other.gameObject.name);
            gameObject.transform.localScale = _startScale;
            gameObject.SetActive(false);
            // TODO add FX

            // If this is the player, decrease player health
            if (other.tag == "Player")
            {
                Debug.Log("Player health decrease by " + _damageAmount);
                _playerHealth.DamagePlayer(_damageAmount);
            }
        }
        
    }
}
