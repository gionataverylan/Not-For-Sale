using UnityEngine;
using UnityEngine.UIElements;

public class UIDeathMenuController : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private RespawnTimer respawnTimer;

    private UIDocument document;
    private VisualElement container;
    private Label counter;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        if (playerHealth == null || respawnTimer == null)
        {
            Debug.LogError("UIDeathMenuController: не назначены playerHealth или respawnTimer", this);
            return;
        }

        counter = document.rootVisualElement.Q<Label>("Counter");
        container = document.rootVisualElement.Q<VisualElement>("Container");

        container.style.display = DisplayStyle.None;

        playerHealth.Died += ShowDeathMenu;
        playerHealth.Respawned += HideDeathMenu;
        respawnTimer.CountChanged += ShowCounter;

    }
    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.Died -= ShowDeathMenu;
            playerHealth.Respawned -= HideDeathMenu;
        }

        if (respawnTimer != null)
        {
            respawnTimer.CountChanged -= ShowCounter;
        }
    }

    private void ShowDeathMenu()
    {
        container.style.display = DisplayStyle.Flex;
    }

    private void HideDeathMenu()
    {
        container.style.display = DisplayStyle.None;
    }

    private void ShowCounter(int count)
    {
        counter.text = count.ToString();
    }
}
