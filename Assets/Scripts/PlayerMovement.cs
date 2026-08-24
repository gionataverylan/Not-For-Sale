using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float sprintSpeed = 12f;
    [SerializeField] private float velocity = -2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float sensitivity = 0.4f;
    [SerializeField] private float pitch = 0f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float jumpBuffer = 0.2f;
    [SerializeField] private float bobSmoothSpeed = 8f;
    [SerializeField] private float bobFrequency = 8f;
    [SerializeField] private float rollAmplitude = 1f;
    [SerializeField] private float pitchAmplitude = 1f;
    [SerializeField] private Transform cameraTarget;


    private float jumpBufferCounter;
    private float timer;
    private float currentRoll;
    private float currentBobPitch;

    private PlayerControler playerControler;
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerControler = new PlayerControler();
    }

    private void OnEnable()
    {
        playerControler.Movement.Enable();
        playerControler.Movement.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        playerControler.Movement.Disable();
        playerControler.Movement.Jump.performed -= OnJump;
    }

    private void Update()
    {
        UpdateTimers();
        OnMove();
        TryJump();
    }

    private void LateUpdate()
    {
        Look();
    }

    private void OnMove()
    {
        Vector2 input = playerControler.Movement.Move.ReadValue<Vector2>();
        bool isSprinting = playerControler.Movement.Sprint.IsPressed();
        
        float newSpeed = isSprinting && input.y > 0 && characterController.isGrounded ? sprintSpeed : speed;

        Vector3 direction = (transform.right * input.x + transform.forward * input.y) * newSpeed * Time.deltaTime;

        direction.y = ApplyGravity() * Time.deltaTime;

        characterController.Move(direction);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpBufferCounter = jumpBuffer;
    }

    private void TryJump()
    {
        if (characterController.isGrounded && jumpBufferCounter > 0)
        {
            velocity = jumpForce;
            jumpBufferCounter = 0f;
        }
    }

    private float ApplyGravity()
    {
        if (characterController.isGrounded && velocity < 0)
        {
            velocity = -2f;
        }
        else
        {
            velocity += gravity * Time.deltaTime;
        }

        return velocity;
    }

    private void Look()
    {
        Vector2 look = playerControler.Movement.Look.ReadValue<Vector2>();
        Vector2 move = playerControler.Movement.Move.ReadValue<Vector2>();

        transform.Rotate(Vector3.up * look.x * sensitivity);

        pitch -= look.y * sensitivity;
        pitch = Mathf.Clamp(pitch, -80f, 80);

        bool isMoving = characterController.isGrounded && move.magnitude > 0.01f;
        BobHead(isMoving);

        cameraTarget.localEulerAngles = new Vector3(pitch + currentBobPitch, 0f, currentRoll);
    }

    private void UpdateTimers()
    {
        jumpBufferCounter -= Time.deltaTime;
    }

    private void BobHead(bool isMoving)
    {
        float targetRoll = 0f;
        float targetBobPitch = 0f;

        if (isMoving)
        {
            timer += Time.deltaTime * bobFrequency;

            targetRoll = Mathf.Sin(timer) * rollAmplitude;
            targetBobPitch = Mathf.Cos(timer * 2) * pitchAmplitude;
        }

        currentRoll = Mathf.Lerp(currentRoll, targetRoll, Time.deltaTime * bobSmoothSpeed);
        currentBobPitch = Mathf.Lerp(currentBobPitch, targetBobPitch, Time.deltaTime * bobSmoothSpeed);
    }
}
