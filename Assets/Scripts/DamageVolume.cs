using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    int _damageVolDecr = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
            // subtracting from score
            player?.DamagePlayer(_damageVolDecr);
            Debug.Log("player hit damage volume!");
        }
    }
}
