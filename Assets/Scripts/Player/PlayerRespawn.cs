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
        playerHealth.Respawned += Respawn;
    }

    private void OnDisable()
    {
        playerHealth.Respawned -= Respawn;
    }

    private void Respawn()
    {
        if (respawnPoint == null)
        {
            Debug.LogWarning("PlayerRespawn: respawnPoint не назначен, игрок остаётся на месте", this);
            return;
        }

        characterController.enabled = false;
        transform.SetPositionAndRotation(
            respawnPoint.position,
            Quaternion.Euler(0f, respawnPoint.eulerAngles.y, 0f));
        characterController.enabled = true;
    }
}
