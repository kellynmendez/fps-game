using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LargeEnemy : MonoBehaviour
{
    #region serialized variables
    [Header("AI")]
    // Distance enemy will stop from the player
    [SerializeField] float _stopDistance = 40f;
    [Header("Bullets")]
    // Velocity of bullet
    [SerializeField] float _velocity = 25f;
    // Bullet pooling object
    [SerializeField] BulletPool _bulletPool;
    // Barrel transform
    [SerializeField] Transform _barrel;
    [Header("Feedback")]
    [SerializeField] AudioClip _shootFX = null;
    #endregion

    #region private variables
    // Target's transform
    private Transform _target;
    // Enemy navmesh
    private NavMeshAgent _agent;
    // Timer for shooting player
    private float _shootTimeStamp = 0f;
    // how often do we want to shoot
    [SerializeField] float _shootInterval = 1f;
    // y offset from player position to target
    private float _offsetY = 5f;
    // audio source
    AudioSource _audioSource;
    #endregion

    public enum EnemyState
    {
        Roam = 0,
        Attack = 1
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _target = FindObjectOfType<PlayerMovement>().transform;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        // Chase player
        _agent.SetDestination(_target.position);
        transform.LookAt(_target.position);
        Vector3 tar = _target.position;
        tar.y += _offsetY;
        _barrel.transform.LookAt(tar);
        _barrel.transform.Rotate(90, 0, 0, Space.Self);

        float distanceAway = Vector3.Distance(transform.position, _target.position);
        // If enemy is moving and has reached range from player
        if (_agent.isStopped && distanceAway < _stopDistance)
        {
            // Shoot at player
            if (Time.time >= _shootTimeStamp + _shootInterval)
            {
                StartCoroutine(ShootAtPlayer());
                _shootTimeStamp = Time.time;
            }
        }

        // If enemy is moving and has reached range from player
        if (!_agent.isStopped && distanceAway < _stopDistance)
        {
            // Stop the enemy
            _agent.isStopped = true;
        }
        // If the enemy has stopped but the player has moved
        else if (_agent.isStopped && distanceAway > _stopDistance)
        {
            // Let the enemy move
            _agent.isStopped = false;
        } 
    }

    private IEnumerator ShootAtPlayer()
    {
        // Wait for look time
        yield return new WaitForSeconds(0.5f);
        transform.LookAt(_target.position);
        // Getting bullet form pool
        GameObject poolGO = _bulletPool.GetPooledObject();
        // Shoot bullet at player
        if (poolGO)
        {
            // Got an object from the pooling system, can use it
            Rigidbody rb = poolGO.GetComponent<Rigidbody>();
            if (rb)
            {
                PlayShootFX();
                Vector3 tar = _target.position;
                tar.y += _offsetY;
                poolGO.transform.LookAt(tar);
                rb.velocity = poolGO.transform.forward * _velocity;
            }
        }
    }

    public void PlayShootFX()
    {
        // play sfx
        if (_audioSource != null && _shootFX != null)
        {
            _audioSource.PlayOneShot(_shootFX, _audioSource.volume);
        }
    }
}
