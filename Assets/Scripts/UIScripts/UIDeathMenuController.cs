using UnityEngine;
using UnityEngine.UIElements;

public class UIDeathMenuController : MonoBehaviour
{
    private UIDocument document;
    private VisualElement container;
    private Button restartButton;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        restartButton = document.rootVisualElement.Q<Button>("Restart");
        container = document.rootVisualElement.Q<VisualElement>("Container");

        playerHealth = FindFirstObjectByType<PlayerHealth>();

        container.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        playerHealth.onDeath += ShowRestartMenu;
        restartButton.clicked += HandleRestart;

    }
    private void OnDisable()
    {
        playerHealth.onDeath -= ShowRestartMenu;
        restartButton.clicked -= HandleRestart;
    }

    private void ShowRestartMenu()
    {
        container.style.display = DisplayStyle.Flex;
    }

    private void HandleRestart()
    {
        Time.timeScale = 1.0f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

}
