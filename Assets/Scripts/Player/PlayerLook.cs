using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private const float MaxPitch = 80f;

    [SerializeField] private float sensitivity = 0.4f;
    [SerializeField] private Transform cameraTarget;

    private float pitch;
    private PlayerInputReader playerInputReader;

    private void Awake()
    {
        playerInputReader = GetComponent<PlayerInputReader>();
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Look()
    {
        Vector2 look = playerInputReader.Look;

        transform.Rotate(Vector3.up * look.x * sensitivity);

        pitch -= look.y * sensitivity;
        pitch = Mathf.Clamp(pitch, -MaxPitch, MaxPitch);

        cameraTarget.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }
}
