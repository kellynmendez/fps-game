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
            EnemyHealth enemy = rayHitInfo.transform.gameObject.GetComponent<EnemyHealth>();
            if (enemy)
            {
                enemy.TakeDamage(_weaponDamage);
                enemy.EnemyKnockback(gameObject.transform.forward);
            }
            

        }

        if (debugRay)
        {
            DebugRay(_cameraController.transform.position, rayDirection * _rayDistance);
        }
    }
}


