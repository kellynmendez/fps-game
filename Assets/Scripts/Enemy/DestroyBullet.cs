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

    private void Awake()
    {
        _startScale = gameObject.transform.localScale;
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
            Debug.Log("set bullet inactive");
            gameObject.transform.localScale = _startScale;
            gameObject.SetActive(false);
            // TODO add FX
        }
        
    }
}
