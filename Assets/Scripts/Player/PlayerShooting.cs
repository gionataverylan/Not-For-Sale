using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private int damageAmount = 50;

    private new Camera camera;
    private PlayerInputReader playerInputReader;

    private void Awake()
    {
        camera = Camera.main;
        playerInputReader = GetComponent<PlayerInputReader>();
    }

    private void OnEnable()
    {
        playerInputReader.Fired += Shoot;
    }

    private void OnDisable()
    {
        playerInputReader.Fired -= Shoot;
    }

    private void Shoot()
    {
        RaycastHit hit;

        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red, 1f);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.TryGetComponent<IDamageable>(out IDamageable damageable))
            { 
                damageable.TakeDamage(damageAmount);
            }
        }
    }
}
