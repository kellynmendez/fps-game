using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController _charController;
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _normalSpeed = 8f;
    [SerializeField] float _sprintSpeed = 25f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float _groundDistance = 0.4f;
    [SerializeField] float _jumpHeight = 3f;
    [SerializeField] LayerMask _groundMask;

    private UIManager _uiManager;
    Vector3 _velocity;
    bool _isGrounded;
    float _currentSpeed;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _currentSpeed = _normalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_uiManager.IsPlayerDead())
        {
            // Player movement
            PlayerMove();
            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                Jump();
            }
            // Sprint
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _currentSpeed = _sprintSpeed;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _currentSpeed = _normalSpeed;
            }
        }
    }

    void PlayerMove()
    {
        // Moving player
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        _charController.Move(move * _currentSpeed * Time.deltaTime);
        // Checking if player is grounded so we can reset velocity
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        // Adding gravity
        _velocity.y += gravity * Time.deltaTime;
        _charController.Move(_velocity * Time.deltaTime);
    }

    void Jump()
    {
        _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * gravity);
    }
}
