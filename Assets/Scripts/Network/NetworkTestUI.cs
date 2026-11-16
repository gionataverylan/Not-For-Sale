using Unity.Netcode;
using UnityEngine;

public class NetworkTestUI : MonoBehaviour
{
    private void OnGUI()
    {
        NetworkManager manager = NetworkManager.Singleton;
        if (manager == null) return;

        GUILayout.BeginArea(new Rect(10, 10, 220, 160));

        if (!manager.IsClient && !manager.IsServer)
        {
            if (GUILayout.Button("Host")) manager.StartHost();
            if (GUILayout.Button("Client")) manager.StartClient();
        }
        else
        {
            GUILayout.Label(manager.IsHost ? "Host" : "Client");

            NetworkObject localPlayer = manager.LocalClient?.PlayerObject;

            if (localPlayer != null && localPlayer.TryGetComponent(out PlayerHealth health))
            {
                GUILayout.Label($"HP: {health.CurrentHealth} / {health.MaxHealth}");
            }
        }

        GUILayout.EndArea();
    }
}