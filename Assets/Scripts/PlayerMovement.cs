using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float velocity = -2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float sensitivity = 0.4f;
    [SerializeField] private float pitch = 0f;
    [SerializeField] private Transform cameraTarget;

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
    }

    private void OnDisable()
    {
        playerControler.Movement.Disable();
    }

    private void Update()
    {
        OnMove();
    }

    private void LateUpdate()
    {
        Look();
    }

    private void OnMove()
    {
        Vector2 input = playerControler.Movement.Move.ReadValue<Vector2>();
        Vector3 direction = (transform.right * input.x + transform.forward * input.y) * speed * Time.deltaTime;

        direction.y = ApplyGravity() * Time.deltaTime;

        characterController.Move(direction);
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

        transform.Rotate(Vector3.up * look.x * sensitivity);

        pitch -= look.y * sensitivity;
        pitch = Mathf.Clamp(pitch, -80f, 80);

        cameraTarget.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }
}
