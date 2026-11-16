using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkSetup : NetworkBehaviour
{
    [SerializeField] private GameObject[] localOnlyObjects;
    [SerializeField] private float spawnSpacing = 2f;

    public override void OnNetworkSpawn()
    {
        bool isLocal = IsOwner;

        GetComponent<PlayerInputReader>().enabled = isLocal;
        GetComponent<PlayerMovement>().enabled = isLocal;
        GetComponent<PlayerLook>().enabled = isLocal;
        GetComponent<HeadBob>().enabled = isLocal;
        GetComponent<PlayerShooting>().enabled = isLocal;

        foreach (GameObject obj in localOnlyObjects)
        {
            obj.SetActive(isLocal);
        }

        if (isLocal)
        {
            MoveToSpawnSlot();
        }
    }

    private void MoveToSpawnSlot()
    {
        CharacterController characterController = GetComponent<CharacterController>();

        characterController.enabled = false;
        transform.position += Vector3.right * spawnSpacing * (int)OwnerClientId;
        characterController.enabled = true;
    }
}