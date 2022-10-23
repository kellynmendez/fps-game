using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] Camera _cameraController;
    [SerializeField] LayerMask _hitLayer;

    [SerializeField] float _rayDistance = 10f;
    [SerializeField] float _rayDuration = 2f;

    private float _weaponDamage = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootRay(true);
        }
    }

    private void DebugRay(Vector3 startPoint, Vector3 endPoint)
    {
        Debug.DrawRay(startPoint, endPoint, Color.red, _rayDuration);
    }


    private void ShootRay(bool debugRay)
    {
        RaycastHit rayHitInfo;
        Vector3 rayDirection = _cameraController.transform.forward;

        if (Physics.Raycast(_cameraController.transform.position, rayDirection, out rayHitInfo, _rayDistance, _hitLayer))
        {
            Debug.Log("<color=green>HIT:</color> You hit the enemy.");
            Enemy enemy = rayHitInfo.transform.gameObject.GetComponent<Enemy>();
            enemy?.TakeDamage(_weaponDamage);
        }
        else
        {
            Debug.Log("<color=red>MISS:</color> You missed.");
        }

        if (debugRay)
        {
            DebugRay(_cameraController.transform.position, rayDirection * _rayDistance);
        }
    }
}


