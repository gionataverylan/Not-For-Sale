using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private PlayerControls controls;
    private PlayerHealth playerHealth;

    public event Action Jumped;
    public event Action Dashed;
    public event Action Fired;

    public Vector2 Move => controls.Movement.Move.ReadValue<Vector2>();
    public Vector2 Look => controls.Movement.Look.ReadValue<Vector2>();
    public bool SprintPressed => controls.Movement.Sprint.IsPressed();

    private void Awake()
    {
        controls = new PlayerControls();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Movement.Jump.performed += OnJump;
        controls.Movement.Dash.performed += OnDash;
        controls.Attack.Fire.performed += OnFire;

        playerHealth.Died += HandleDeath;
        playerHealth.Respawned += HandleRespawn;
    }

    private void OnDisable()
    {
        controls.Disable();

        controls.Movement.Jump.performed -= OnJump;
        controls.Movement.Dash.performed -= OnDash;
        controls.Attack.Fire.performed -= OnFire;

        playerHealth.Died -= HandleDeath;
        playerHealth.Respawned -= HandleRespawn;
    }
    private void OnDestroy()
    {
        controls.Dispose();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Jumped?.Invoke();
    }

    private void OnDash(InputAction.CallbackContext context) 
    {
        Dashed?.Invoke();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        Fired?.Invoke();
    }

    private void HandleDeath()
    {
        controls.Disable();
    }

    private void HandleRespawn()
    {
        controls.Enable();
    }

}
