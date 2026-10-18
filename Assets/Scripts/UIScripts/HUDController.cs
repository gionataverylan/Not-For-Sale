using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    private UIDocument document;
    private ProgressBar healthBar;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    private void OnEnable()
    {
        playerHealth.onHealthChanged += HandleChangeHP;

        healthBar = document.rootVisualElement.Q<ProgressBar>("HealthBar");
        healthBar.value = playerHealth.MaxHealth;
        healthBar.highValue = playerHealth.MaxHealth;
    }

    private void OnDisable()
    {
        playerHealth.onHealthChanged -= HandleChangeHP;
    }

    private void HandleChangeHP(int newHealth)
    {
        healthBar.value = newHealth;
    }
}
