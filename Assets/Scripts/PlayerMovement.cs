using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float velocity = 1f;
    
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

    private void OnMove()
    {
            Vector2 input = playerControler.Movement.Move.ReadValue<Vector2>();
            Vector3 direction = (transform.right * input.x + transform.forward * input.y) * speed * Time.deltaTime;

            characterController.Move(direction);
    }
}
