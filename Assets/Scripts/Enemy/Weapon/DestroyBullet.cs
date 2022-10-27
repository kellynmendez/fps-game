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
        gameObject.transform.localScale = _startScale;
        Debug.Log("set inactive");
        gameObject.SetActive(false);
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "EnemyBody")
        {
            gameObject.transform.localScale = _startScale;
            gameObject.SetActive(false);
            // TODO add FX

            // If this is the player, decrease player health
            if (other.tag == "Player")
            {
                _playerHealth.DamagePlayer(_damageAmount);
            }
        }
        
    }
}
