using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormalEnemy : MonoBehaviour
{
    #region serialized variables
    [Header("AI")]
    // How far the random position will be from current position
    [SerializeField] float _randPosRange = 8f;
    // Distance enemy will spot the player
    [SerializeField] float _rangeOfAwareness = 15f;
    // Distance enemy will stop from the player
    [SerializeField] float _stopDistance = 6f;
    // How much time enemy waits before changing direcitons in roam
    [SerializeField] float _waitRoamTime = 1f;
    [Header("Bullets")]
    // Velocity of bullet
    [SerializeField] float _velocity = 20f;
    // Bullet pooling object
    [SerializeField] BulletPool _bulletPool;
    #endregion

    #region private variables
    // Target's transform
    private Transform _target;
    // Enemy navmesh
    private NavMeshAgent _agent;
    // enemy state
    private EnemyState _state;
    // Timer for shooting player
    private float _shootTimeStamp = 0f;
    // how often do we want to shoot
    private float _shootInterval = 1f;
    //roamed to random position, time to change
    bool _roamReached = false;
    // Random position to roam to
    Vector3 _randPosition;
    // Edges of plane
    float _planeX = 80f;
    float _planeZ = 80f;
    // enemy is waiting
    bool _isWaiting = false;
    #endregion

    public enum EnemyState
    {
        Roam = 0,
        Attack = 1
    }

    private void Awake()
    {
        _target = FindObjectOfType<PlayerMovement>().transform;
        _agent = GetComponent<NavMeshAgent>();
        _state = EnemyState.Roam;
    }

    private void Start()
    {
        _randPosition = RandomPosition();
        _agent.SetDestination(_randPosition);
    }

    private void Update()
    {
        DetectPlayer();

        // Enemy is roaming (not within range)
        if (_state == EnemyState.Roam)
        {
            if (_roamReached && !_isWaiting)
            {
                // Setting desination
                _randPosition = RandomPosition();
                _agent.SetDestination(_randPosition);
                _roamReached = false;
            }
            else
            {
                // Checking if reached destination
                Vector3 currPos = new Vector3(transform.position.x, 0, transform.position.z);
                float distAway = Vector3.Distance(currPos, _randPosition);
                if (distAway <= 0.1 && !_roamReached)
                {
                    _roamReached = true;
                    // Have enemy wait before moving to next random position
                    StartCoroutine(Wait());
                }
            }
        }
        // Enemy is attacking
        else
        {
            _agent.SetDestination(_target.position);
            transform.LookAt(_target.position);
        }
    }

    private Vector3 RandomPosition()
    {
        // set random destination
        Vector3 randPos = new Vector3(
            transform.position.x + Random.Range(-_randPosRange, _randPosRange), 
            0,
            transform.position.z + Random.Range(-_randPosRange, _randPosRange));
        // Make sure random position is not out of bounds of plane
        if (randPos.x > _planeX)
            randPos.x = _planeX;
        else if (randPos.x < -_planeX)
            randPos.x = -_planeX;
        if (randPos.z > _planeZ)
            randPos.z = _planeZ;
        else if (randPos.z < -_planeZ)
            randPos.z = -_planeZ;
        return randPos;
    }

    private void DetectPlayer()
    {
        float distanceAway = Vector3.Distance(transform.position, _target.position);
        // If player is within range, change to attack
        if (!(_state == EnemyState.Attack) && distanceAway < _rangeOfAwareness)
        {
            _state = EnemyState.Attack;
        }
        // Otherwise, if not in range, make enemy roam again
        else if (!(_state == EnemyState.Roam) && distanceAway > _rangeOfAwareness)
        {
            _state = EnemyState.Roam;
            _randPosition = RandomPosition();
            _agent.SetDestination(_randPosition);
            _roamReached = false;
        }

        if (_agent.isStopped && distanceAway < _stopDistance)
        {
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

    private IEnumerator Wait()
    {
        _isWaiting = true;
        _agent.isStopped = true;
        yield return new WaitForSeconds(_waitRoamTime);
        _isWaiting = false;
        _agent.isStopped = false;
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
                rb.velocity = poolGO.transform.forward * _velocity;
            }
        }
    }
}
