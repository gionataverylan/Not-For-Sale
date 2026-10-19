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
        transform.position = respawnPoint.position;
        characterController.enabled = true;
    }
}
