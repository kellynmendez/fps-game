using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _defaultHealth = 3f;
    private Transform _target;
    NavMeshAgent _agent;

    //timer so we update tracking a little less
    private float _timeStamp = 0f;
    //how often do we want to track
    private float _timeOffset = 0.2f; // doing it every 0.2 seconds
    // distance enemy will spot the player
    private float _rangeOfAwareness = 5f;
    // enemy health
    private float _health;

    private void Awake()
    {
        _target = FindObjectOfType<PlayerMovement>().transform;
        _agent = GetComponent<NavMeshAgent>();
        _health = _defaultHealth;
    }

    private void Update()
    {
        if (Time.time >= _timeStamp + _timeOffset)
        {
            _agent.SetDestination(_target.position);
            _timeStamp = Time.time;
            DetectPlayer();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        _health -= damageAmount;
        Debug.Log($"Enemy health is now {_health}");

        if (_health == 0)
        {
            Debug.Log("Enemy has died!");
            gameObject.SetActive(false);
            _health = _defaultHealth;
        }
    }

    private void DetectPlayer()
    {
        float distanceAway = Vector3.Distance(transform.position, _target.position);
        // If enemy is moving and has reached range from player
        if (!_agent.isStopped && distanceAway < _rangeOfAwareness)
        {
            // Stop the enemy
            _agent.isStopped = true;
            Debug.Log("Player is within range");
        }
        // If the enemy has stopped but the player has moved
        else if (_agent.isStopped && distanceAway > _rangeOfAwareness)
        {
            // Let the enemy move
            _agent.isStopped = false;
        }
    }
}
