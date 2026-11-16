using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private int damageAmount = 50;
    [SerializeField] private float range = 100f;

    private PlayerInputReader playerInputReader;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerInputReader = GetComponent<PlayerInputReader>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        playerInputReader.Fired += OnFired;
    }

    private void OnDisable()
    {
        playerInputReader.Fired -= OnFired;
    }

    private void OnFired()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        FireRpc(ray.origin, ray.direction);
    }

    [Rpc(SendTo.Server)]
    private void FireRpc(Vector3 origin, Vector3 direction)
    {
        if (playerHealth.IsDead) return;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, range))
        {
            if (hit.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damageAmount);
            }
        }
    }
}