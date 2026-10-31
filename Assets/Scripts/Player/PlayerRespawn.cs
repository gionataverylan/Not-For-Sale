using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private PlayerHealth playerHealth;
    private CharacterController characterController;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerHealth.onRespawn += Respawn;
    }

    private void OnDisable()
    {
        playerHealth.onRespawn -= Respawn;
    }

    private void Respawn()
    {
        characterController.enabled = false;
        transform.SetPositionAndRotation(
            respawnPoint.position,
            Quaternion.Euler(0f, respawnPoint.eulerAngles.y, 0f));
        characterController.enabled = true;
    }
}
