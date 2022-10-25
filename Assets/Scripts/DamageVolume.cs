using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    int _damageVolDecr = 10;
    bool _playerInCollider = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _playerInCollider = true;
            PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
            while (_playerInCollider)
            {
                // subtracting from score
                player?.DamagePlayer(_damageVolDecr);
                Debug.Log("player hit damage volume!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _playerInCollider = false;
        }
    }
}
