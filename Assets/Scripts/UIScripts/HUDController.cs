using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    private UIDocument document;
    private ProgressBar healthBar;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        if (playerHealth == null)
        {
            Debug.LogError("HUDController: не назначен playerHealth", this);
            return;
        }

        healthBar = document.rootVisualElement.Q<ProgressBar>("HealthBar");
        healthBar.value = playerHealth.MaxHealth;
        healthBar.highValue = playerHealth.MaxHealth;

        playerHealth.HealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.HealthChanged -= HandleHealthChanged;
        }
    }

    private void HandleHealthChanged(int newHealth)
    {
        healthBar.value = newHealth;
    }
}
