using UnityEngine;
using UnityEngine.UIElements;

public class UIDeathMenuController : MonoBehaviour
{
    private UIDocument document;
    private VisualElement container;
    private Label counter;
    private PlayerHealth playerHealth;
    private RespawnTimer timer;

    private void Awake()
    {
        document = GetComponent<UIDocument>();

        playerHealth = FindFirstObjectByType<PlayerHealth>();
        timer = FindFirstObjectByType<RespawnTimer>();
    }

    private void OnEnable()
    {
        counter = document.rootVisualElement.Q<Label>("Counter");
        container = document.rootVisualElement.Q<VisualElement>("Container");

        container.style.display = DisplayStyle.None;

        playerHealth.Died += ShowRestartMenu;
        playerHealth.Respawned += HideRestartMenu;
        timer.CountChanged += ShowCounter;

    }
    private void OnDisable()
    {
        playerHealth.Died -= ShowRestartMenu;
        playerHealth.Respawned -= HideRestartMenu;
        timer.CountChanged -= ShowCounter;
    }

    private void ShowRestartMenu()
    {
        container.style.display = DisplayStyle.Flex;
    }

    private void HideRestartMenu()
    {
        container.style.display = DisplayStyle.None;
    }

    private void ShowCounter(int count)
    {
        counter.text = count.ToString();
    }
}
