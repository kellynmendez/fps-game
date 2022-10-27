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
    // Pickup object
    InvinciblePowerup _powerUp;
    // respawning?
    bool _respawning = false;
    // collectible or pickup?
    bool col = true;

    void Awake()
    {
        _currPickup = Instantiate(_poolObject);
        _currPickup.transform.position = transform.position;
        _collectible = _currPickup.GetComponent<Collectible>();
        if (!_collectible)
        {
            _powerUp = _currPickup.GetComponent<InvinciblePowerup>();
            col = false;
        }
    }

    private void Update()
    {
        if (col)
        {
            if (!_collectible.IsActive() && !_respawning)
            {
                StartCoroutine(SpawnPickup());
            }
        }
        else
        {
            if (!_powerUp.IsActive() && !_respawning)
            {
                StartCoroutine(SpawnPickup());
            }
        }
    }

    IEnumerator SpawnPickup()
    {
        _respawning = true;
        yield return new WaitForSeconds(_respawnTime);
        if (col)
        {
            _collectible.Reactivate();
        }
        else
        {
            _powerUp.Reactivate();
        }
        _respawning = false;
    }
}
