using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] Camera _cameraController;
    [SerializeField] LayerMask _hitLayer;

    [SerializeField] float _rayDistance = 10f;
    [SerializeField] float _rayDuration = 2f;
    [SerializeField] float _cooldownTime = 10f;

    [Header("Feedback")]
    [SerializeField] AudioClip _shootFX;
    [SerializeField] AudioClip _rocketShootFX;

    private float _weaponDamage = 1f;
    private bool _rocketMode = false;
    private bool _rocketCoolingDown = false;
    private UIManager _uiManager;
    private AudioSource _audioSource;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !_rocketMode)
        {
            ShootRay(true);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && _rocketMode && !_rocketCoolingDown)
        {
            LaunchRocket(true);
            StartCoroutine(RocketCooldown());
        }
        // Tab to switch between weapons
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!_rocketMode)
            {
                SetupRocketMode();
            }
            else
            {
                DisableRocketMode();
            }
        }
    }

    private void DebugRay(Vector3 startPoint, Vector3 endPoint)
    {
        Debug.DrawRay(startPoint, endPoint, Color.red, _rayDuration);
    }


    private void ShootRay(bool debugRay)
    {
        PlayShootFX();
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

    private void LaunchRocket(bool debugRay)
    {
        PlayRocketShootFX();
        RaycastHit rayHitInfo;
        Vector3 rayDirection = _cameraController.transform.forward;

        if (Physics.Raycast(_cameraController.transform.position, rayDirection, out rayHitInfo, _rayDistance, _hitLayer))
        {
            EnemyHealth enemy = rayHitInfo.transform.gameObject.GetComponent<EnemyHealth>();
            if (enemy)
            {
                enemy.RocketChainKill();
            }
        }

        if (debugRay)
        {
            DebugRay(_cameraController.transform.position, rayDirection * _rayDistance);
        }
    }

    IEnumerator RocketCooldown()
    {
        _rocketCoolingDown = true;
        _uiManager.SetRocketCrosshairActiveState(false);
        yield return new WaitForSeconds(_cooldownTime);
        _uiManager.SetRocketCrosshairActiveState(true);
        _rocketCoolingDown = false;
    }

    private void SetupRocketMode()
    {
        _rocketMode = true;
        _uiManager.SetNormalCrosshairActiveState(false);
        if (!_rocketCoolingDown)
        {
            _uiManager.SetRocketCrosshairActiveState(true);
        }
    }

    private void DisableRocketMode()
    {
        _rocketMode = false;
        _uiManager.SetRocketCrosshairActiveState(false);
        _uiManager.SetNormalCrosshairActiveState(true);
    }

    public void PlayShootFX()
    {
        // play sfx
        if (_audioSource != null && _shootFX != null)
        {
            _audioSource.PlayOneShot(_shootFX, _audioSource.volume);
        }
    }

    public void PlayRocketShootFX()
    {
        // play sfx
        if (_audioSource != null && _rocketShootFX != null)
        {
            _audioSource.PlayOneShot(_rocketShootFX, _audioSource.volume);
        }
    }
}


