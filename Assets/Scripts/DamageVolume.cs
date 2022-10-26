using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    private int _damageVolDecr = 5;
    private bool _playerInCollider = false;
    private float _timeStamp = 0f;
    private float _interval = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("triggered");
            _playerInCollider = true;
            PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
            if (player)
            {
                Debug.Log("damage volume coroutine");
                StartCoroutine(DamagePlayer(player));
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

    IEnumerator DamagePlayer(PlayerHealth player)
    {
        while (_playerInCollider)
        {
            if (Time.time >= _timeStamp + _interval)
            {
                player.DamagePlayer(_damageVolDecr);
                Debug.Log("player hit damage volume!");
                _timeStamp = Time.time;
            }
            yield return null;
        }
    }
}
