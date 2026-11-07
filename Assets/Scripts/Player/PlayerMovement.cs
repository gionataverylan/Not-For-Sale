using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private const float GroundedVelocity = -2f;
    private const float InputThreshold = 0.01f;

    [SerializeField] private float speed = 8f;
    [SerializeField] private float sprintSpeed = 12f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField, FormerlySerializedAs("sprintJumpMultiplier")] private float sprintJumpForce = 10f;
    [SerializeField] private float jumpBuffer = 0.2f;
    [SerializeField] private float dashTimer = 0.15f;
    [SerializeField] private float fallMultiplier = 1.5f;
    [SerializeField] private float dashCooldown = 0.35f;

    private float velocity = GroundedVelocity;
    private float jumpBufferCounter;
    private float airborneSpeed;
    private float dashTimerCounter;
    private float dashCooldownCounter;
    private bool isSprinting;
    private Vector3 dashDirection;

    private CharacterController characterController;
    private PlayerInputReader playerInputReader;
    private PlayerHealth playerHealth;

    public bool IsGrounded => characterController.isGrounded;
    public bool IsSprinting => isSprinting;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInputReader = GetComponent<PlayerInputReader>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        playerInputReader.Jumped += OnJump;
        playerInputReader.Dashed += OnDash;
        playerHealth.Respawned += ResetAfterDeath;
    }

    private void OnDisable()
    {
        playerInputReader.Jumped -= OnJump;
        playerInputReader.Dashed -= OnDash;
        playerHealth.Respawned -= ResetAfterDeath;
    }

    private void Update()
    {
        UpdateTimers();
        OnMove();
        TryJump();
    }

    private void OnMove()
    {
        Vector2 input = playerInputReader.Move;
        bool sprintPressed = playerInputReader.SprintPressed;

        isSprinting = sprintPressed && input.y > 0 && characterController.isGrounded;

        float newSpeed = isSprinting ? sprintSpeed : speed;
        float horizontalSpeed = characterController.isGrounded == false ? airborneSpeed : newSpeed;

        Vector3 directionMove = (transform.right * input.x + transform.forward * input.y);

        Vector3 moveVector = dashTimerCounter > 0 ? dashDirection * dashSpeed : directionMove * horizontalSpeed;

        Vector3 direction = moveVector * Time.deltaTime;
        direction.y = ApplyGravity() * Time.deltaTime;

        characterController.Move(direction);
    }

    private void OnJump()
    {
        jumpBufferCounter = jumpBuffer;
    }

    private void OnDash()
    {
        Vector2 input = playerInputReader.Move;

        if(dashCooldownCounter <= 0)
        {
            dashTimerCounter = dashTimer;
            dashCooldownCounter = dashCooldown;

            if (input.sqrMagnitude > InputThreshold && characterController.isGrounded)
            {
                dashDirection = (transform.right * input.x + transform.forward * input.y).normalized;
            }
            else
            {
                dashDirection = transform.forward;
            }
        }  
    }

    private void TryJump()
    {

        if (characterController.isGrounded && jumpBufferCounter > 0)
        {
            velocity = isSprinting ? sprintJumpForce : jumpForce;
            airborneSpeed = isSprinting ? sprintSpeed : speed;
            jumpBufferCounter = 0f;
        }
    }

    private float ApplyGravity()
    {

        if (characterController.isGrounded && velocity < 0)
        {
            velocity = GroundedVelocity;
        }
        else
        {
            float newGravity = velocity <= 0 ? fallMultiplier * gravity : gravity;
            velocity += newGravity * Time.deltaTime;
        }

        return velocity;
    }

    private void ResetAfterDeath()
    {
        velocity = GroundedVelocity;
        dashTimerCounter = 0f;
        dashCooldownCounter = 0f;
        jumpBufferCounter = 0f;
    }

    private void UpdateTimers()
    {
        jumpBufferCounter -= Time.deltaTime;
        dashTimerCounter -= Time.deltaTime;
        dashCooldownCounter -= Time.deltaTime;
    }
}
