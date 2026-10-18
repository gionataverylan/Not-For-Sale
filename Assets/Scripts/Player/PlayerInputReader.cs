using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private PlayerControler playerControler;
    private PlayerHealth playerHealth;

    public event Action Jumped;
    public event Action Dashed;
    public Vector2 Move => playerControler.Movement.Move.ReadValue<Vector2>();
    public Vector2 Look => playerControler.Movement.Look.ReadValue<Vector2>();

    public bool SprintPressed => playerControler.Movement.Sprint.IsPressed();

    private void Awake()
    {
        playerControler = new PlayerControler();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        playerControler.Movement.Enable();
        playerControler.Movement.Jump.performed += OnJump;
        playerControler.Movement.Dash.performed += OnDash;
        playerHealth.onDeath += HandleDeath;
        playerHealth.onRespawn += HandleRespawn;
    }

    private void OnDisable()
    {
        playerControler.Movement.Disable();
        playerControler.Movement.Jump.performed -= OnJump;
        playerControler.Movement.Dash.performed -= OnDash;
        playerHealth.onDeath -= HandleDeath;
        playerHealth.onRespawn -= HandleRespawn;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Jumped?.Invoke();
    }

    private void OnDash(InputAction.CallbackContext context) 
    {
        Dashed?.Invoke();
    }

    private void HandleDeath()
    {
        playerControler.Movement.Disable();
    }

    private void HandleRespawn()
    {
        playerControler.Movement.Enable();
    }

}
