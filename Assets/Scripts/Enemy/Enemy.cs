using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Starting default health
    [SerializeField] float _defaultHealth = 3f;
    // How far the random position will be from current position
    [SerializeField] float _randPosRange = 8f;
    // Distance enemy will spot the player
    [SerializeField] float _rangeOfAwareness = 15f;
    // Distance enemy will stop from the player
    [SerializeField] float _stopDistance = 6f;
    // How much time enemy waits before changing direcitons in roam
    [SerializeField] float _waitTime = 3f;
    // Bullet pooling object
    [SerializeField] BulletPool _bulletPool;

    // Target's transform
    private Transform _target;
    // Enemy navmesh
    NavMeshAgent _agent;
    // enemy state
    EnemyState _state;
    // timer so we update tracking a little less
    private float _timeStamp = 0f;
    // how often do we want to track
    private float _timeOffset = 0.2f; // doing it every 0.2 seconds
    // enemy health
    private float _health;
    //roamed to random position, time to change
    bool _roamReached = false;
    // Random position to roam to
    Vector3 _randPosition;
    // Edges of plane
    float _planeX = 80f;
    float _planeZ = 80f;
    // shoot distance offset from camera
    float _shootOffset = 1.5f;

    public enum EnemyState
    {
        Roam = 0,
        Attack = 1
    }

    private void Awake()
    {
        _target = FindObjectOfType<PlayerMovement>().transform;
        _agent = GetComponent<NavMeshAgent>();
        _health = _defaultHealth;
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
            if (_roamReached)
            {
                // Setting desination
                _randPosition = RandomPosition();
                _agent.SetDestination(_randPosition);
                _roamReached = false;
            }
            else
            {
                // Checking if reached destination
                Vector3 currPos = new Vector3(
                    transform.position.x,
                    0,
                    transform.position.z);
                float distAway = Vector3.Distance(currPos, _randPosition);
                if (distAway <= 1)
                {
                    _roamReached = true;
                    Debug.Log("waiting");
                }
            }
        }
        // Enemy is attacking
        else
        {
            if (Time.time >= _timeStamp + _timeOffset)
            {
                Debug.Log($"{gameObject.name} seeking player");
                _agent.SetDestination(_target.position);
                _timeStamp = Time.time;
            }
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

        // If enemy is moving and has reached range from player
        if (!_agent.isStopped && distanceAway < _stopDistance)
        {
            // Stop the enemy
            _agent.isStopped = true;
            Debug.Log("Player is within range");
            StartCoroutine(LerpBulletToPlayer());
        }
        // If the enemy has stopped but the player has moved
        else if (_agent.isStopped && distanceAway > _stopDistance)
        {
            // Let the enemy move
            _agent.isStopped = false;
        }
    }

    private IEnumerator LerpBulletToPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        transform.LookAt(_target.position);
        bool reachedGoal = false;
        GameObject poolGO = _bulletPool.GetPooledObject();
        Vector3 shotTarget = _target.position;
        shotTarget.y += _shootOffset;
        // Use the distance between the start and end to determine how long to lerp for.
        float dist = Vector3.Distance(poolGO.transform.position, shotTarget);
        float duration = dist;
        float elapsedTime = 0;
        while (!reachedGoal)
        {
            elapsedTime += Time.deltaTime;
            float percentComplete = elapsedTime / duration;
            poolGO.transform.position = Vector3.Lerp(poolGO.transform.position,
                                 shotTarget,
                                 percentComplete);
            yield return null;
        }
    }
}
