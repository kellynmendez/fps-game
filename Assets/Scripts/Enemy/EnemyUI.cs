using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] float _maxHealth = 3f;
    // Slider
    [SerializeField] Slider _slider;
    // canvas
    [SerializeField] Canvas _canvas;
    // Camera
    CameraLook _camera;

    private void Awake()
    {
        _slider.value = _maxHealth;
        _camera = FindObjectOfType<CameraLook>();
    }

    private void Update()
    {
        _canvas.transform.LookAt(_camera.transform.position);
    }

    public void SetHealthBar(float health)
    {
        _slider.value = health;
    }

}
