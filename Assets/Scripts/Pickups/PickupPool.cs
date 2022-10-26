using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPool : MonoBehaviour
{
    // The game object that the pool will hold
    [SerializeField] GameObject _poolObject;
    // Respawn time
    [SerializeField] float _respawnTime = 15f;
    // current pickup
    private GameObject _currPickup;
    // Collectible object
    Collectible _collectible;
    // respawning?
    bool _respawning = false;

    void Start()
    {
        _currPickup = Instantiate(_poolObject);
        _currPickup.transform.position = transform.position;
        _collectible = _currPickup.GetComponent<Collectible>();
    }

    private void Update()
    {
        if (!_collectible.IsActive() && !_respawning)
        {
            StartCoroutine(SpawnPickup());
        }
    }

    IEnumerator SpawnPickup()
    {
        _respawning = true;
        yield return new WaitForSeconds(_respawnTime);
        _collectible.Reactivate();
        _respawning = false;
    }
}
