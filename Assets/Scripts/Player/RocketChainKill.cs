using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketChainKill : MonoBehaviour
{
    private ParticleSystem _particles;
    private List<ParticleCollisionEvent> _collisionEvents;

    private void Awake()
    {
        _particles = GetComponent<ParticleSystem>();
        _collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = _particles.GetCollisionEvents(other, _collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Debug.Log("enemy hit by particle");
            EnemyHealth enemy = _collisionEvents[i].colliderComponent.gameObject.GetComponent<EnemyHealth>();
            enemy?.RocketChainKill();
        }
    }
}
