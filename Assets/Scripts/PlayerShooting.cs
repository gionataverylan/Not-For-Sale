using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 direction;
    private new Camera camera;
    private PlayerCombat playerCombat;
    //private RaycastHit hit;
    private Ray ray;

    private void Awake()
    {
        camera = Camera.main;
        playerCombat = new PlayerCombat();
    }

    private void OnEnable()
    {
        playerCombat.Attack.Enable();
        playerCombat.Attack.Fire.performed += Shoot;
    }

    private void OnDisable()
    {
        playerCombat.Attack.Disable();
        playerCombat.Attack.Fire.performed -= Shoot;
    }

    private void Update()
    {
        ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.name);
        }
        else
        {
            Debug.Log("miss");
        }

    }
}
